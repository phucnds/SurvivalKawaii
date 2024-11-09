using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDepnedency
{
    [SerializeField] private int baseMaxHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI txtHealth;

    private float health;
    private int maxHealth;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoveryValue = .1f;
    private float healthRecoverySpeed;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    public static Action<Vector2> onAttackDodged;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isHitCritical)
    {
        if (health >= maxHealth) return;

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Math.Min(lifeStealValue, maxHealth - health);

        health += healthToAdd;
        UpdateUI();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (health >= maxHealth) return;
        RecoverHealth();
    }

    private void RecoverHealth()
    {
        healthRecoveryTimer += Time.deltaTime;

        if (healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd = Mathf.Min(healthRecoveryValue, maxHealth - health);
            health += healthToAdd;

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        slider.value = health / maxHealth;
        txtHealth.text = $"{(int)health} / {maxHealth}";
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }


        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 1000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;

        UpdateUI();

        if (health <= 0)
        {
            PassAway();
        }

    }

    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f) < dodge;
    }

    private void PassAway()
    {
        GameHandler.Instance.SetGameState(GameState.GAMEOVER);
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);
        health = maxHealth;
        UpdateUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = Math.Max(0.0001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}
