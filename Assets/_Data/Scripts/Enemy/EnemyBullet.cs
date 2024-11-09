using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private RangeEnemyAttack rangeEnemyAttack;

    private int damage;
    private Rigidbody2D rb;
    private Collider2D col2D;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
    }

    public void ShootBullet(int damage, Vector2 dir)
    {
        Invoke(nameof(Release), 5);
        this.damage = damage;
        transform.right = dir;
        rb.velocity += dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            CancelInvoke();
            player.TakeDamage(1);
            col2D.enabled = false;
            Release();
        }
    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Reload()
    {
        rb.velocity = Vector2.zero;
        col2D.enabled = true;
    }

    private void Release()
    {
        if (!gameObject.activeSelf) return;
        rangeEnemyAttack.ReleaseBullet(this);
    }
}
