using System;
using UnityEngine;

public class Cash : DroppableCurrency
{
    public static Action<Cash> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);
    }
}
