using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level / 3;

        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();

        foreach (KeyValuePair<Stat, float> kvp in weaponData.BaseStat)
        {
            if (weaponData.Prefab.GetType() != typeof(RangeWeapon) && kvp.Key == Stat.Range)
            {
                calculatedStats.Add(kvp.Key, kvp.Value);
            }
            else
            {
                calculatedStats.Add(kvp.Key, kvp.Value * multiplier);
            }
        }

        return calculatedStats;
    }

    public static int GetPurchasePrice(WeaponDataSO weaponDataSO, int level)
    {
        float multiplier = 1 + (float)level / 3;
        return (int)(weaponDataSO.PurchasePrice * multiplier);
    }

    public static int GetRecyclePrice(WeaponDataSO weaponDataSO, int level)
    {
        float multiplier = 1 + (float)level / 3;
        return (int)(weaponDataSO.RecyclePrice * multiplier);
    }
}
