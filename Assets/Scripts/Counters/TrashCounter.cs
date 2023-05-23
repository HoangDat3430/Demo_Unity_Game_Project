using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnDroppingObject;

    new public static void ResetStatic()
    {
        OnDroppingObject = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnDroppingObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
