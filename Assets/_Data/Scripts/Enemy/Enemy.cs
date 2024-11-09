using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected ParticleSystem passAwayParticles;
    [SerializeField] protected SpriteRenderer sprEnemy;
    [SerializeField] protected SpriteRenderer sprSpawnIndicator;
    [SerializeField] protected float playerDetectionRadius = .5f;

    [SerializeField] protected int maxHealth;

    public static Action<int, Vector2, bool> onDamageTaken;
    public static Action<Vector2> onPassedAway;

    protected int health;

    protected EnemyMovement movement;
    protected Player player;
    protected Collider2D col2d;
    protected bool hasSpawned = false;

    protected virtual void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        col2d = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        health = maxHealth;
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, Auto-destroying...");
            Destroy(gameObject);
        }
        StarSpawnSequence();
    }

    public virtual void TakeDamage(int damage, bool isCriticalHit)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
        {
            PassAway();
        }
    }

    public void PassAway()
    {
        onPassedAway?.Invoke(transform.position);
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();
        Destroy(gameObject);
    }

    private void StarSpawnSequence()
    {
        SetRenderersVisibility(false);
        Vector3 targetScale = sprSpawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(sprSpawnIndicator.gameObject, targetScale, .3f).setLoopPingPong(4).setOnComplete(SpawnSequenceCompleted);
    }

    private void SetRenderersVisibility(bool visibility)
    {
        col2d.enabled = visibility;
        sprEnemy.enabled = visibility;
        sprSpawnIndicator.enabled = !visibility;
    }

    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true);
        hasSpawned = true;
        movement.StorePlayer(player);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
