using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager
{
    const string statIconDataPath = "Data/Stat Icons";
    const string objectDataPath = "Data/Objects/";
    const string weaponDataPath = "Data/Weapons/";
    const string characterDataPath = "Data/Characters/";

    private static StatIcon[] statIcons;

    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
        {
            if (stat == statIcon.stat)
            {
                return statIcon.sprite;
            }
        }

        return null;
    }


    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get
        {
            if (objectDatas == null)
            {
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDataPath);
            }
            return objectDatas;
        }
        private set { }
    }

    public static ObjectDataSO GetRandomObject()
    {
        return Objects[Random.Range(0, Objects.Length)];
    }

    private static WeaponDataSO[] weaponDataSOs;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDataSOs == null)
            {
                weaponDataSOs = Resources.LoadAll<WeaponDataSO>(weaponDataPath);
            }
            return weaponDataSOs;
        }
        private set { }
    }

    public static WeaponDataSO GetRandomWeapon()
    {
        return Weapons[Random.Range(0, Weapons.Length)];
    }

    private static CharacterDataSO[] characterDataSOs;
    public static CharacterDataSO[] Characters
    {
        get
        {
            if (characterDataSOs == null)
            {
                characterDataSOs = Resources.LoadAll<CharacterDataSO>(characterDataPath);
            }
            return characterDataSOs;
        }
        private set { }
    }
}
