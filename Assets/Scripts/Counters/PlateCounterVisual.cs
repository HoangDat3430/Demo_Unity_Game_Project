using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] Transform counterTopPoint;
    [SerializeField] Transform plateVisual;
    [SerializeField] PlateCounter plateCounter;

    private List<GameObject> plateVisualList;

    private void Awake()
    {
        plateVisualList = new List<GameObject>();
    }
    private void Start()
    {
        plateCounter.OnSpawn += PlateCounter_OnSpawn;
        plateCounter.OnRemoved += PlateCounter_OnRemoved;
    }

    private void PlateCounter_OnRemoved(object sender, System.EventArgs e)
    {
        GameObject lastPlateSpawned = plateVisualList[plateVisualList.Count - 1];
        plateVisualList.Remove(lastPlateSpawned);
        Destroy(lastPlateSpawned);
    }

    private void PlateCounter_OnSpawn(object sender, System.EventArgs e)
    {
        Transform plateSpawnedTransform = Instantiate(plateVisual, counterTopPoint);
        
        float plateOffsetY = 0.1f;
        plateSpawnedTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualList.Count ,0);

        plateVisualList.Add(plateSpawnedTransform.gameObject);
    }
}
