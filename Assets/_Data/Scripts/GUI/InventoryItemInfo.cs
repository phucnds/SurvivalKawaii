using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtItemName;
    [SerializeField] private TextMeshProUGUI txtRecyclePrice;

    [SerializeField] private Image container;
    [SerializeField] private Transform statsParent;

    [field: SerializeField] public Button RecycleButton { get; private set; }
    [field: SerializeField] public Button CombineButton { get; private set; }

    public void Configure(Weapon weapon)
    {
        Configure(
            weapon.WeaponData.Sprite,
            weapon.WeaponData.Name,
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.Level)
        );

        CombineButton.gameObject.SetActive(true);
        CombineButton.interactable = WeaponMerger.Instance.CanMerge(weapon);
        CombineButton.onClick.RemoveAllListeners();
        CombineButton.onClick.AddListener(WeaponMerger.Instance.Merge);
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure(
            objectData.Sprite,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStat
        );

        CombineButton.gameObject.SetActive(false);
    }

    private void Configure(Sprite itemIcon, string itemName, Color clrContainer, int recyclePrice, Dictionary<Stat, float> stats)
    {
        icon.sprite = itemIcon;
        txtItemName.text = itemName;
        txtItemName.color = clrContainer;
        container.color = clrContainer;
        txtRecyclePrice.text = recyclePrice.ToString();
        StatContainerManager.GenerateContainers(stats, statsParent);
    }

}
