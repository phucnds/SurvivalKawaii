using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{

    private RangeEnemyAttack attack;

    override protected void Awake()
    {
        base.Awake();
        attack = GetComponent<RangeEnemyAttack>();
    }

    override protected void Start()
    {
        base.Start();
        attack.StorePlayer(player);
    }

    private void Update()
    {
        if (!hasSpawned) return;
        ManageAttack();

        
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > playerDetectionRadius)
        {
            movement.FollowPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        attack.AutoAnim();
    }
}
