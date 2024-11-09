using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private Transform hitDetectTransform;

    private BoxCollider2D boxCollider2D;
    private List<Enemy> damagedEnemy = new List<Enemy>();

    private enum State
    {
        Idle,
        Attack
    }

    private State state;


    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        boxCollider2D = hitDetectTransform.GetComponent<BoxCollider2D>();

    }

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                RotateWeapon();
                break;

            case State.Attack:
                Attack();
                break;
        }

    }

    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        anim.Play("Attack");
        state = State.Attack;
        damagedEnemy.Clear();

        anim.speed = 1f / attackDelay;

        if (!AudioManager.Instance.IsSFXOn) return;
        PlayAttackSound();
    }

    private void StopAttack()
    {
        damagedEnemy.Clear();
        state = State.Idle;
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectTransform.position, boxCollider2D.bounds.size, hitDetectTransform.localEulerAngles.z, layerMaskEnemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            if (!damagedEnemy.Contains(enemy))
            {
                int damage = GetDamage(out bool isCriticalHit);
                enemy.TakeDamage(damage, isCriticalHit);
                damagedEnemy.Add(enemy);
            }
        }
    }

    private void RotateWeapon()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            ManageAttack();
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * animLerp);
        IncrementAttackTimer();

    }

    private void ManageAttack()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));

        attackDelay /= 1 + (playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);
        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += playerStatsManager.GetStatValue(Stat.CriticalPercent);
        range += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}
