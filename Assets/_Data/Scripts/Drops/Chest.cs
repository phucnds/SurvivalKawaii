using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : DroppableCurrency
{
    public static Action<Chest> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
