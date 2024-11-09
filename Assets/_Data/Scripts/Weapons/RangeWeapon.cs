using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [SerializeField] private Bullet bulletPrefabs;
    [SerializeField] private Transform shootingPoint;

    private ObjectPool<Bullet> bulletPool;
    public static Action onBulletShoot;

    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private Bullet CreateFunction()
    {
        Bullet bulletInstance = Instantiate(bulletPrefabs, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);
        return bulletInstance;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void Update()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.right;
        Vector3 scale = Vector3.one;

        if (closestEnemy != null)
        {
            scale = closestEnemy.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(y: -1);
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.right = targetUpVector;

            ManageShooting();
        }

        transform.localScale = scale;
        transform.right = Vector3.Lerp(transform.right, targetUpVector, Time.deltaTime * animLerp);
    }

    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shooting();
        }
    }

    private void Shooting()
    {
        Bullet bullet = bulletPool.Get();
        int damage = GetDamage(out bool isCriticalHit);
        bullet.ShootBullet(damage, transform.right, isCriticalHit);

        if (!AudioManager.Instance.IsSFXOn) return;
        PlayAttackSound();
        onBulletShoot?.Invoke();
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
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
