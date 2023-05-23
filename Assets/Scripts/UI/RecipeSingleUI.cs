using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform ingredientIconTemplate;


    private void Awake()
    {
        ingredientIconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeName.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child != ingredientIconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform ingredientTransform = Instantiate(ingredientIconTemplate, iconContainer);
            ingredientTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
            ingredientTransform.gameObject.SetActive(true);
        }
    }
}
