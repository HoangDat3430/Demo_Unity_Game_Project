using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStatic()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCutting;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    
    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            // The counter already has an object 
            if (!player.HasKitchenObject())
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
        else
        // Clear counter does not have anything
        {
            if (player.HasKitchenObject() && HasCuttingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Player carrying an object and this object has a recipe to 
                player.GetKitchenObject().SetKitchenObjectParent(this);

                cuttingProgress = GetKitchenObject().GetKitchenObjectSO().timer;
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
    }
    public override void InteractAlternative(Player player)
    {
        if (HasKitchenObject() && HasCuttingRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            GetKitchenObject().GetKitchenObjectSO().timer++;    
            cuttingProgress = GetKitchenObject().GetKitchenObjectSO().timer;

            OnCutting?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);   
            // There is an object and it have a recipe to cut
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOuputFromInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOuputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO CuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (CuttingRecipeSO != null)
        {
            return CuttingRecipeSO.output;
        }
        return null;
    }

    private bool HasCuttingRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO CuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return CuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
} 
