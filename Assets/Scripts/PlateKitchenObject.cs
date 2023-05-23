using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnConpletingEventArgs> OnAddingIngredients;
    public class OnConpletingEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObject;

    private List<KitchenObjectSO> ingredients;

    private void Awake()
    {
        ingredients = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (ingredients.Contains(kitchenObjectSO))
        {
            Debug.Log("Already has this ingredient!");
            return false;
        }
        else
        {
            if (!validKitchenObject.Contains(kitchenObjectSO))
            {
                Debug.Log("Invalid Ingredient!");
                return false;
            }
        }
        ingredients.Add(kitchenObjectSO);
        OnAddingIngredients?.Invoke(this, new OnConpletingEventArgs
        {   
            kitchenObjectSO = kitchenObjectSO
        });
        Debug.Log("Adding ingredients successfully!");
        return true;
    }

    public List<KitchenObjectSO> GetIngredientList()
    {
        return ingredients;
    }
}
