using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearCounter : BaseCounter
{
    public static ClearCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            // The counter has an object 
            if (!player.HasKitchenObject())
            {
                // Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                // Player carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                // Player carrying a plate
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player carrying something but not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // Counter has a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
        else
        // Clear counter does not have anything
        {
            if (player.HasKitchenObject())
            {
                // Player carrying an object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
    }

}
