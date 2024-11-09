using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private EnemyBullet bulletPrefabs;

    [SerializeField] private int damage = 1;
    [SerializeField] private float attackFrequency;

    private Player player;
    private float attackDelay;
    private float attackTimer;

    private Vector2 gizmosDirection;

    private ObjectPool<EnemyBullet> bulletPool;

    private void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }


    private EnemyBullet CreateFunction()
    {
        EnemyBullet enemyBulletInstance = Instantiate(bulletPrefabs, shootPoint.position, Quaternion.identity);
        enemyBulletInstance.Configure(this);

        return enemyBulletInstance;
    }

    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }


    public void AutoAnim()
    {
        ManageShooting();
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
        Vector2 direction = (player.GetCenter() - shootPoint.position).normalized;
        gizmosDirection = direction;

        EnemyBullet bulletGO = bulletPool.Get();
        bulletGO.ShootBullet(damage, direction);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(shootPoint.position, (Vector2)shootPoint.position + gizmosDirection * 5);
    }

    public void ReleaseBullet(EnemyBullet enemyBullet)
    {
        bulletPool.Release(enemyBullet);
    }
}
