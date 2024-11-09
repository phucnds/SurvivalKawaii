using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private PlayerObjects playerObjects;
    [SerializeField] private PlayerWeapons playerWeapons;

    [SerializeField] private Transform inventoryItemParent;
    [SerializeField] private Transform pauseInventoryItemParent;
    [SerializeField] private InventoryItem inventoryItem;

    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;


    private void Awake()
    {
        ShopManager.onItemPurchase += ItemPurchaseCallback;
        WeaponMerger.onMerge += WeaponMergerCallback;
        GameHandler.onGamePaused += Configure;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchase -= ItemPurchaseCallback;
        WeaponMerger.onMerge -= WeaponMergerCallback;
        GameHandler.onGamePaused -= Configure;
    }

    public void GameStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
        }
    }

    private void ItemPurchaseCallback() => Configure();

    private void Configure()
    {
        inventoryItemParent.Clear();
        pauseInventoryItemParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;
            InventoryItem item = Instantiate(inventoryItem, inventoryItemParent);
            item.Configure(weapons[i], i, () => ShowItemInfo(item));

            InventoryItem items = Instantiate(inventoryItem, pauseInventoryItemParent);
            items.Configure(weapons[i], i, null);
        }

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItem item = Instantiate(inventoryItem, inventoryItemParent);
            item.Configure(objectDatas[i], () => ShowItemInfo(item));

            InventoryItem items = Instantiate(inventoryItem, inventoryItemParent);
            items.Configure(objectDatas[i], null);
        }
    }

    private void ShowItemInfo(InventoryItem item)
    {
        if (item.Weapon != null)
        {
            ShowWeaponInfo(item.Weapon, item.Index);
        }
        else
        {
            ShowObjectInfo(item.Object);
        }
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfo.Configure(weapon);
        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));
        shopManagerUI.ShowItemInfo();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Configure(objectData);
        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));
        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectData)
    {
        playerObjects.RecycleObject(objectData);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);
        Configure();
        shopManagerUI.HideItemInfo();
    }

    private void WeaponMergerCallback(Weapon weapon)
    {
        Configure();
        itemInfo.Configure(weapon);
    }
}
