using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Player player;

    private void Update()
    {
         if (player == null) return;
        // FollowPlayer();

        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
    }

    public void StorePlayer(Player _player)
    {
        this.player = _player;
    }

    public void FollowPlayer()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Vector2 targetPos = (Vector2)transform.position + dir * moveSpeed * Time.deltaTime;
        transform.position = targetPos;
    }


}
