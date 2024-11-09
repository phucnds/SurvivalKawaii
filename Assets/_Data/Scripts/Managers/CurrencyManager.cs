using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabsil.Sijil;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int PremiumCurrency { get; private set; }

    const string PremiumCurrencyKey = "premiumCurrency";

    public static CurrencyManager Instance;

    public static Action onUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Candy.onCollected += CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;

        // AddPremiumCurrency(PlayerPrefs.GetInt(PremiumCurrencyKey), false);

    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected -= CashCollectedCallback;
    }

    private void Start()
    {
        UpdateTexts();
    }

    [NaughtyAttributes.Button]
    private void AddCurrency500() => AddCurrency(500);

    [NaughtyAttributes.Button]
    private void AddPremiumCurrency500() => AddPremiumCurrency(500);

    public void Load()
    {
        if (Sijil.TryLoad(this, PremiumCurrencyKey, out object premiumCurrencyValue))
        {
            AddPremiumCurrency((int)premiumCurrencyValue, false);
        }
        else
        {
            AddPremiumCurrency(0, false);
        }
    }

    public void Save()
    {
        Sijil.Save(this, PremiumCurrencyKey, PremiumCurrency);
    }

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();
    }

    public void AddPremiumCurrency(int amount, bool save = true)
    {
        PremiumCurrency += amount;
        UpdateVisuals();

        // if (save) PlayerPrefs.SetInt(PremiumCurrencyKey, PremiumCurrency);
    }

    private void UpdateVisuals()
    {
        UpdateTexts();
        onUpdated?.Invoke();

        Save();
    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText text in currencyTexts)
        {
            text.UpdateText(Currency.ToString());
        }

        PremiumCurrencyText[] premiumCurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (PremiumCurrencyText text in premiumCurrencyTexts)
        {
            text.UpdateText(PremiumCurrency.ToString());
        }
    }

    public bool HasEnoughCurrency(int price) => Currency >= price;
    public bool HasEnoughPremiumCurrency(int price) => PremiumCurrency >= price;
    public void UseCurrency(int price) => AddCurrency(-price);
    public void UsePremiumCurrency(int price) => AddPremiumCurrency(-price);
    private void CandyCollectedCallback(Candy candy) => AddCurrency(1);
    private void CashCollectedCallback(Cash cash) => AddPremiumCurrency(1);
}
