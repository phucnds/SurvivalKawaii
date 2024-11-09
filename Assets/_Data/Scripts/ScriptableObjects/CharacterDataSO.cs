using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "DemonSlayer/CharacterDataSO", order = 0)]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }


    [Space(10)]
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float healthRecoverySpeed;
    [SerializeField] private float armor;
    [SerializeField] private float luck;
    [SerializeField] private float dodge;
    [SerializeField] private float lifeSteal;

    public Dictionary<Stat, float> BaseStat
    {
        get
        {
            return new Dictionary<Stat, float>
            {
                {Stat.Attack, attack},
                {Stat.AttackSpeed, attackSpeed},
                {Stat.CriticalChance, criticalChance},
                {Stat.CriticalPercent, criticalPercent},
                {Stat.MoveSpeed, moveSpeed},
                {Stat.MaxHealth, maxHealth},
                {Stat.Range, range},
                {Stat.HealthRecoverySpeed, healthRecoverySpeed},
                {Stat.Armor, armor},
                {Stat.Luck, luck},
                {Stat.Dodge, dodge},
                {Stat.LifeSteal, lifeSteal},

            };
        }
        private set { }
    }

    public Dictionary<Stat, float> NonNeutralStats
    {
        get
        {
            Dictionary<Stat, float> nonNeutralStats = new Dictionary<Stat, float>();

            foreach (KeyValuePair<Stat, float> kvp in BaseStat)
            {
                if (kvp.Value != 0)
                {
                    nonNeutralStats.Add(kvp.Key, kvp.Value);
                }
            }

            return nonNeutralStats;
        }
        private set { }
    }

}