using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnSpawn;
    public event EventHandler OnRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnTimerMax = 4f;
    private int plateSpawnedAmount;
    private int plateSpawnedMax = 4;

    private void Start()
    {
        spawnPlateTimer = 0f;
        plateSpawnedAmount = 0;
    }

    private void Update()
    {
        if(plateSpawnedAmount < plateSpawnedMax && GameHandler.Instance.GetState() != GameHandler.State.waitingToStart)
        {
            spawnPlateTimer += Time.deltaTime;
            if(spawnPlateTimer > spawnTimerMax)
            {
                OnSpawn?.Invoke(this, new EventArgs());
                plateSpawnedAmount++;
                spawnPlateTimer = 0f;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (plateSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnRemoved?.Invoke(this, EventArgs.Empty);
                plateSpawnedAmount--;
            }
        }
    }
}
