using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] weaponPositions;
    [SerializeField] private Transform weaponParent;

    public bool TryAddWeapon(WeaponDataSO weaponData, int level)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null) continue;
            weaponPositions[i].AssignWeapon(weaponData.Prefab, level);
            return true;
        }

        return false;
    }

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();

        foreach (WeaponPosition weaponPosition in weaponPositions)
        {
            if (weaponPosition.Weapon == null)
                weapons.Add(null);
            else
                weapons.Add(weaponPosition.Weapon);
        }

        return weapons.ToArray();
    }

    public void RecycleWeapon(int index)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (i != index) continue;

            int recyclePrice = weaponPositions[i].Weapon.GetRecyclePrice();
            CurrencyManager.Instance.AddCurrency(recyclePrice);
            weaponPositions[i].RemoveWeapon();
            return;
        }
    }
}
