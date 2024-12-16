using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerStatsDepnedency
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick joystick;
    [Header("Settings")]
    [SerializeField] private float baseMoveSpeed = .1f;
    private float moveSpeed = .1f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
