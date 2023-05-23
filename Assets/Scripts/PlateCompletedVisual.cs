using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateCompletedVisual : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<Transform> prefabs;

    private void Awake()
    {
       foreach (Transform t in prefabs)
        {
            t.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        plateKitchenObject.OnAddingIngredients += PlateKitchenObject_OnCompleting;
    }

    private void PlateKitchenObject_OnCompleting(object sender, PlateKitchenObject.OnConpletingEventArgs e)
    {
        foreach (Transform t in prefabs)
        {
            if (t.name == e.kitchenObjectSO.prefab.name)
            { 
                t.gameObject.SetActive(true);       
            }
        }
    }
}
