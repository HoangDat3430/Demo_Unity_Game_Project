using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnSpawnedRecipe;
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFail;


    public static DeliveryManager Instance { get; private set; }


    [SerializeField] private RecipeListSO recipeListSO;


    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeSOTime;
    private float spawnRecipeSOTimeMax = 4f;
    private int waitingRecipeSOMax = 4;
    private int correctRecipeDeliveredAmount;
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
        spawnRecipeSOTime = 0f;
        correctRecipeDeliveredAmount = 0;
    }

    private void Update()
    {
        if (waitingRecipeSOList.Count < waitingRecipeSOMax && GameHandler.Instance.GetState() != GameHandler.State.waitingToStart)
        {
            spawnRecipeSOTime += Time.deltaTime;
            if (spawnRecipeSOTime >= spawnRecipeSOTimeMax)
            {
                spawnRecipeSOTime = 0f;
                RecipeSO recipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(recipeSO);
                OnSpawnedRecipe?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    public void Delivered(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO recipeSO = waitingRecipeSOList[i];
            if (plateKitchenObject.GetIngredientList().Count == recipeSO.kitchenObjectSOList.Count)
            {
                bool recipeMatched = true;
                foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetIngredientList())
                {
                    bool ingredientMatched = false;
                    foreach (KitchenObjectSO waitingKitchenObjectSO in recipeSO.kitchenObjectSOList)
                    {
                        if (kitchenObjectSO == waitingKitchenObjectSO)
                        {
                            ingredientMatched = true;
                            break;
                        }
                    }
                    if (!ingredientMatched)
                    {
                        recipeMatched = false;
                    }
                }
                if (recipeMatched)
                {
                    correctRecipeDeliveredAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            } 
        }
        OnDeliveryFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return this.waitingRecipeSOList;
    }

    public int GetRecipeSuccessAmount()
    {
        return correctRecipeDeliveredAmount;
    }
}
