using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask enemyLayerMark;

    private int damage;
    private Rigidbody2D rb;
    private Collider2D col2D;
    private RangeWeapon rangeWeapon;

    private Enemy target;
    private bool isCriticalHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
    }

    public void ShootBullet(int damage, Vector2 dir, bool isCriticalHit)
    {
        Invoke(nameof(Release), 5);
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;
        transform.right = dir;
        rb.velocity += dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (target != null) return;

        if (IsInLayerMask(other.gameObject.layer, enemyLayerMark))
        {

            target = other.GetComponent<Enemy>();
            CancelInvoke();
            other.GetComponent<Enemy>().TakeDamage(damage, isCriticalHit);
            // col2D.enabled = false;
            Release();
        }
    }

    private bool IsInLayerMask(int layer, LayerMask enemyLayerMark)
    {
        return (enemyLayerMark.value & (1 << layer)) != 0;
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Reload()
    {
        target = null;
        rb.velocity = Vector2.zero;
        col2D.enabled = true;
    }

    private void Release()
    {
        if (!gameObject.activeSelf) return;
        rangeWeapon.ReleaseBullet(this);
    }
}
