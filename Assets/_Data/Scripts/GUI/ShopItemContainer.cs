using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private Image LockImage;
    [SerializeField] private Sprite lockSprite, unlockSprite;
    [SerializeField] public Button purchaseButton;
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outLine;

    private int weaponLevel;

    public bool IsLocked { get; private set; }
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    public static Action<ShopItemContainer, int> onPurchase;

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdateCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdateCallback;
    }

    private void Start()
    {
        UpdateLockVisual();
    }

    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;
        weaponLevel = level;
        icon.sprite = weaponData.Sprite;
        txtName.text = weaponData.Name;

        outLine.color = ColorHolder.GetOutLineColor(level);
        Color imageColor = ColorHolder.GetColor(level);
        txtName.color = imageColor;
        int price = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        txtPrice.text = price.ToString();

        foreach (Image img in levelDependentImages)
        {
            img.color = imageColor;
        }

        purchaseButton.onClick.AddListener(Purchase);
        Dictionary<Stat, float> calculatedStat = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainer(calculatedStat);
        purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(price);

    }

    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;
        icon.sprite = objectData.Sprite;
        txtName.text = objectData.Name;

        outLine.color = ColorHolder.GetOutLineColor(objectData.Rarity);
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        txtName.color = imageColor;
        txtPrice.text = objectData.Price.ToString();

        foreach (Image img in levelDependentImages)
        {
            img.color = imageColor;
        }
        purchaseButton.onClick.AddListener(Purchase);
        ConfigureStatContainer(objectData.BaseStat);
        purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(objectData.Price);

    }

    private void ConfigureStatContainer(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.GenerateContainers(stats, statContainersParent);
    }

    public void LockButtonCallBack()
    {
        IsLocked = !IsLocked;
        UpdateLockVisual();
    }

    private void UpdateLockVisual()
    {
        LockImage.sprite = IsLocked ? lockSprite : unlockSprite;
    }

    private void Purchase()
    {
        onPurchase?.Invoke(this, weaponLevel);
    }

    private void CurrencyUpdateCallback()
    {
        int itemPrice;
        if (WeaponData != null)
        {
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        }
        else
        {
            itemPrice = ObjectData.Price;
        }

        purchaseButton.interactable = CurrencyManager.Instance.HasEnoughCurrency(itemPrice);
    }
}
