using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;

    [SerializeField] private KitchenObjectSO plateObjectSO;
    private float plateSpawnTimer;
    private const float plateSpawnTimerMax = 4f;
    private int platesSpawnedAmount = 0;
    private int platesSpawnedMax = 4;

    private void Update()
    {
        plateSpawnTimer += Time.deltaTime;
        if (plateSpawnTimer > plateSpawnTimerMax)
        {
            plateSpawnTimer = 0;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedMax)
            {
                platesSpawnedAmount++;
                OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject()) //The player does not have a kitchen object
        {
            if (platesSpawnedAmount > 0) //There is at least one plate on the counter
            {
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateObjectSO, player);
                OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
