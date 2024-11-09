using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestObjecContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtRecyclePrice;
    [SerializeField] private Transform statContainersParent;

    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }

    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outLine;

    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Sprite;
        txtName.text = objectData.Name;

        outLine.color = ColorHolder.GetOutLineColor(objectData.Rarity);
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        txtName.color = imageColor;
        txtRecyclePrice.text = objectData.RecyclePrice.ToString();

        foreach (Image img in levelDependentImages)
        {
            img.color = imageColor;
        }

        ConfigureStatContainer(objectData.BaseStat);
    }

    private void ConfigureStatContainer(Dictionary<Stat, float> calculatedStat)
    {
        StatContainerManager.GenerateContainers(calculatedStat, statContainersParent);
    }
}
