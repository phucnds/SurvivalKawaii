using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image container;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public Weapon Weapon { get; private set; }
    public ObjectDataSO Object { get; private set; }

    public int Index { get; private set; }

    public void Configure(Color containerColor, Sprite itemIcon, Action callback)
    {
        container.color = containerColor;
        icon.sprite = itemIcon;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }

    public void Configure(Weapon weapon, int index, Action callback)
    {
        Weapon = weapon;
        Index = index;
        container.color = ColorHolder.GetColor(weapon.Level);
        icon.sprite = weapon.WeaponData.Sprite; ;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }

    public void Configure(ObjectDataSO objectData, Action callback)
    {
        Object = objectData;
        container.color = ColorHolder.GetColor(objectData.Rarity);
        icon.sprite = objectData.Sprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }
}
