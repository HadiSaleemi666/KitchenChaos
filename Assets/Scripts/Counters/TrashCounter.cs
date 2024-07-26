using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnTrashedObject;
   public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnTrashedObject?.Invoke(this, EventArgs.Empty);
        }
    }

    new public static void ResetStaticData()
    {
        OnTrashedObject = null;
    }
}
