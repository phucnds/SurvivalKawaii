using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    [SerializeField] private PlayerWeapons playerWeapons;

    private List<Weapon> weaponsToMerge = new List<Weapon>();

    public static WeaponMerger Instance;
    public static Action<Weapon> onMerge;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 4) return false;

        weaponsToMerge.Clear();
        weaponsToMerge.Add(weapon);
        Weapon[] weapons = playerWeapons.GetWeapons();

        foreach (Weapon playerWeapon in weapons)
        {
            if (playerWeapon == null) continue;
            if (playerWeapon == weapon) continue;
            if (playerWeapon.WeaponData.Name != weapon.WeaponData.Name) continue;
            if (playerWeapon.Level != weapon.Level) continue;

            weaponsToMerge.Add(playerWeapon);
            return true;
        }

        return false;
    }

    public void Merge()
    {
        if (weaponsToMerge.Count < 2) return;

        DestroyImmediate(weaponsToMerge[1].gameObject);

        weaponsToMerge[0].Upgrade();

        Weapon weapon = weaponsToMerge[0];
        weaponsToMerge.Clear();

        onMerge?.Invoke(weapon);
    }
}
