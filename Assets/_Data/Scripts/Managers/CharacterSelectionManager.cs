using System;
using System.Collections;
using System.Collections.Generic;
using Tabsil.Sijil;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour, IWantToBeSaved
{
    [SerializeField] private Transform characterButtonParent;
    [SerializeField] private CharacterButton characterButtonPrefab;
    [SerializeField] private Image centerCharacterImage;
    [SerializeField] private CharacterInfoPanel characterInfoPanel;

    private CharacterDataSO[] characterDatas;
    private int selectedCharacterIndex;
    private List<bool> unlockedStates = new List<bool>();
    private int lastSelectedCharacter;

    private const string unlockedStatesKey = "unlockedStatesKey";
    private const string lastSelectedCharacterKey = "lastSelectedCharacterKey";

    public static Action<CharacterDataSO> onCharacterSelected;

    private void Start()
    {
        characterInfoPanel.Button.onClick.RemoveAllListeners();
        characterInfoPanel.Button.onClick.AddListener(PurchaseCharacterSelected);

        CharacterSelectedCallback(lastSelectedCharacter);
    }

    private void Initialize()
    {
        characterButtonParent.Clear();

        for (int i = 0; i < characterDatas.Length; i++)
        {
            CreateCharacterButton(i);
        }
    }

    private void CreateCharacterButton(int index)
    {
        CharacterDataSO characterData = characterDatas[index];
        CharacterButton characterButton = Instantiate(characterButtonPrefab, characterButtonParent);
        characterButton.Configure(characterData.Sprite, unlockedStates[index]);
        characterButton.Button.onClick.RemoveAllListeners();
        characterButton.Button.onClick.AddListener(() => CharacterSelectedCallback(index));
    }

    private void CharacterSelectedCallback(int index)
    {
        selectedCharacterIndex = index;
        CharacterDataSO characterData = characterDatas[index];
        centerCharacterImage.sprite = characterData.Sprite;
        characterInfoPanel.Configure(characterData, unlockedStates[index]);

        if (unlockedStates[index])
        {
            lastSelectedCharacter = index;
            characterInfoPanel.Button.interactable = false;
            Save();

            onCharacterSelected?.Invoke(characterData);
        }
        else
        {
            characterInfoPanel.Button.interactable = CurrencyManager.Instance.HasEnoughPremiumCurrency(characterData.PurchasePrice);
        }
    }

    private void PurchaseCharacterSelected()
    {
        int price = characterDatas[selectedCharacterIndex].PurchasePrice;
        CurrencyManager.Instance.UsePremiumCurrency(price);
        unlockedStates[selectedCharacterIndex] = true;

        characterButtonParent.GetChild(selectedCharacterIndex).GetComponent<CharacterButton>().UnLock();
        CharacterSelectedCallback(selectedCharacterIndex);

        Save();
    }

    public void Load()
    {
        characterDatas = ResourcesManager.Characters;
        for (int i = 0; i < characterDatas.Length; i++)
        {
            unlockedStates.Add(i == 0);
        }

        if (Sijil.TryLoad(this, unlockedStatesKey, out object unlockedStatesObject))
        {
            unlockedStates = (List<bool>)unlockedStatesObject;
        }

        if (Sijil.TryLoad(this, lastSelectedCharacterKey, out object lastSelectedCharacterObject))
        {
            lastSelectedCharacter = (int)lastSelectedCharacterObject;
        }

        Initialize();

    }

    public void Save()
    {
        Sijil.Save(this, unlockedStatesKey, unlockedStates);
        Sijil.Save(this, lastSelectedCharacterKey, lastSelectedCharacter);
    }
}
