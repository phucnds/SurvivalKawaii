using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeContainer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI txtUpgradeName;
    [SerializeField] private TextMeshProUGUI txtUpgradeValue;
    
    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite icon, string upgradeName, string upgradeValue)
    {
        image.sprite = icon;
        txtUpgradeName.text = upgradeName;
        txtUpgradeValue.text = upgradeValue;
    }
}
