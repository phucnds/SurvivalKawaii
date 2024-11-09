using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private GameObject priceContainer;
    [SerializeField] private Transform statsParent;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(CharacterDataSO characterData, bool unlocked)
    {
        txtName.text = characterData.Name;
        txtPrice.text = characterData.PurchasePrice.ToString();
        priceContainer.SetActive(!unlocked);
        StatContainerManager.GenerateContainers(characterData.NonNeutralStats, statsParent);
    }

}
