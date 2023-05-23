using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }


    private void Start()
    {
        DeliveryManager.Instance.OnSpawnedRecipe += DeliveryManager_OnSpawnedRecipe;
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        UpdateVisual();
    }

    private void DeliveryManager_OnSpawnedRecipe(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }


    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<RecipeSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
