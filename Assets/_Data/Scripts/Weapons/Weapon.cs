using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDepnedency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [field: SerializeField] public int Level { get; private set; }

    [SerializeField] protected float range = 3f;
    [SerializeField] protected float animLerp = 12f;
    [SerializeField] protected LayerMask layerMaskEnemy;
    [SerializeField] protected float attackDelay;

    protected Animator anim;
    protected int damage = 1;
    protected float attackTimer;
    protected int criticalChance;
    protected float criticalPercent;

    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = WeaponData.AttackSound;
    }

    protected void PlayAttackSound()
    {
        audioSource.pitch = Random.Range(.95f, 1.05f);
        audioSource.Play();
    }

    protected virtual Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, layerMaskEnemy);
        if (enemies.Length <= 0) return null;

        float minDistance = range;
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyCheck = enemies[i].GetComponent<Enemy>();
            float distanceToEnemy = Vector2.Distance(transform.position, enemyCheck.transform.position);
            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyCheck;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    protected virtual int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 100) <= criticalChance)
        {
            // Debug.Log(criticalPercent);
            isCriticalHit = true;
            return Mathf.RoundToInt(damage * criticalPercent);
        }

        return damage;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);

    protected virtual void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedStat = WeaponStatsCalculator.GetStats(WeaponData, Level);

        damage = Mathf.RoundToInt(calculatedStat[Stat.Attack]);
        attackDelay = 1f / calculatedStat[Stat.AttackSpeed];

        criticalChance = Mathf.RoundToInt(calculatedStat[Stat.CriticalChance]);
        criticalPercent = calculatedStat[Stat.CriticalPercent];

        if (WeaponData.Prefab.GetType() == typeof(RangeWeapon))
            range = calculatedStat[Stat.Range];
    }

    public void UpgradeTo(int weaponLevel)
    {
        Level = weaponLevel;
        ConfigureStats();
    }

    public int GetRecyclePrice() => WeaponStatsCalculator.GetPurchasePrice(WeaponData, Level);

    public void Upgrade() => UpgradeTo(Level + 1);

}