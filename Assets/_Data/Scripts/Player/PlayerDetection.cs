using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private CircleCollider2D collectableCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ICollectable>(out ICollectable candy))
        {
            if (!other.IsTouching(collectableCollider)) return;
            candy.Collect(GetComponent<Player>());
        }
    }
}
