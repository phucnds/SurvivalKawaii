using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    public void Collect(Player player)
    {
        if (collected) return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    private IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPos = transform.position;

        while (timer < 1)
        {
            transform.position = Vector2.Lerp(initialPos, player.GetCenter(), timer);
            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();

}
