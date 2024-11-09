using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private Transform containerParent;
    [SerializeField] private ShopItemContainer shopItemPrefab;

    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private TextMeshProUGUI txtRerollPrice;

    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    public static Action onItemPurchase;

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdateCallback;
        ShopItemContainer.onPurchase += ItemPurchaseCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdateCallback;
        ShopItemContainer.onPurchase -= ItemPurchaseCallback;
    }

    public void GameStateChangeCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
        {
            Configure();
            UpdateRerollVisuals();
        }
    }

    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < containerParent.childCount; i++)
        {
            ShopItemContainer shopItem = containerParent.GetChild(i).GetComponent<ShopItemContainer>();
            if (!shopItem.IsLocked)
            {
                toDestroy.Add(shopItem.gameObject);
            }
        }

        while (toDestroy.Count > 0)
        {
            Transform t = toDestroy[0].transform;
            t.SetParent(null);
            Destroy(t.gameObject);
            toDestroy.RemoveAt(0);
        }

        int containerToAdd = 5 - containerParent.childCount;
        int weaponContainersCount = Random.Range(0, containerToAdd);
        int objectContainerCount = containerToAdd - weaponContainersCount;

        for (int i = 0; i < weaponContainersCount; i++)
        {
            ShopItemContainer weapon = Instantiate(shopItemPrefab, containerParent);
            WeaponDataSO weaponData = ResourcesManager.GetRandomWeapon();
            weapon.Configure(weaponData, 0);
        }

        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer item = Instantiate(shopItemPrefab, containerParent);
            ObjectDataSO randomObject = ResourcesManager.GetRandomObject();
            item.Configure(randomObject);
        }
    }

    public void Reroll()
    {
        Configure();
        CurrencyManager.Instance.UseCurrency(rerollPrice);
    }

    private void UpdateRerollVisuals()
    {
        txtRerollPrice.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(rerollPrice);
    }

    private void CurrencyUpdateCallback()
    {
        UpdateRerollVisuals();
    }

    private void ItemPurchaseCallback(ShopItemContainer container, int level)
    {
        if (container.WeaponData != null)
        {
            TryPurchaseWeapon(container, level);
        }
        else
        {
            PurchaseObject(container);
        }
    }

    private void PurchaseObject(ShopItemContainer container)
    {
        playerObjects.AddObject(container.ObjectData);
        CurrencyManager.Instance.UseCurrency(container.ObjectData.Price);
        Destroy(container.gameObject);

        onItemPurchase?.Invoke();
    }

    private void TryPurchaseWeapon(ShopItemContainer container, int level)
    {
        if (playerWeapons.TryAddWeapon(container.WeaponData, level))
        {
            int price = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, level);
            CurrencyManager.Instance.UseCurrency(price);

            Destroy(container.gameObject);
        }

        onItemPurchase?.Invoke();
    }
}
