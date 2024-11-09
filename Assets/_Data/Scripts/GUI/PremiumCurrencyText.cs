using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PremiumCurrencyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCurrency;

    public void UpdateText(string currencyText)
    {
        txtCurrency.text = currencyText;
    }
}