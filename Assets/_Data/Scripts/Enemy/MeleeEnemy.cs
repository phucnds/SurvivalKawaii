using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float attackFrequency;
    [SerializeField] protected int damage = 1;

    private float attackDelay;
    private float attackTimer;

    override protected void Start()
    {
        base.Start();
        attackDelay = 1f / attackFrequency;
    }

    private void Update()
    {
        if (!hasSpawned) return;
        if (attackTimer >= attackDelay)
        {
            TryAttack();
        }
        else
        {
            Wait();
        }

        movement.FollowPlayer();
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        attackTimer = 0;
        player.TakeDamage(damage);
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < playerDetectionRadius)
        {
            Attack();
        }
    }
}
