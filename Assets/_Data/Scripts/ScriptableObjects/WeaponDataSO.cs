using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "DemonSlayer/WeaponDataSO", order = 1)]
public class WeaponDataSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public int PurchasePrice { get; private set; }
    [field: SerializeField] public int RecyclePrice { get; private set; }
    [field: SerializeField] public AudioClip AttackSound { get; private set; }
    [field: SerializeField] public Weapon Prefab { get; private set; }

    [Space(10)]
    [HorizontalLine]
    [SerializeField] private float attack;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;
    [SerializeField] private float criticalPercent;
    [SerializeField] private float range;

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
                {Stat.Range, range},
            };
        }
        private set { }
    }

    public float GetStatValue(Stat stat)
    {
        foreach (KeyValuePair<Stat, float> kvp in BaseStat)
        {
            if (kvp.Key == stat)
                return kvp.Value;
        }

        return 0;
    }
}