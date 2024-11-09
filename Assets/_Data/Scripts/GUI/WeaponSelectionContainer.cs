using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private Transform statContainersParent;

    [field: SerializeField] public Button Button { get; private set; }

    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Image outLine;

    public void Configure(WeaponDataSO weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        txtName.text = weaponData.Name;

        outLine.color = ColorHolder.GetOutLineColor(level);
        Color imageColor = ColorHolder.GetColor(level);
        txtName.color = imageColor;
       
        foreach (Image img in levelDependentImages)
        {
            img.color = imageColor;
        }

        Dictionary<Stat, float> calculatedStat = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainer(calculatedStat);

    }

    private void ConfigureStatContainer(Dictionary<Stat, float> calculatedStat)
    {
        StatContainerManager.GenerateContainers(calculatedStat, statContainersParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f);
    }


}
