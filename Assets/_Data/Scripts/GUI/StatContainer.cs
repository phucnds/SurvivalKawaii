using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtValue;

    public void Configure(Sprite sprite, string name, float value, bool isColor = false)
    {
        icon.sprite = sprite;
        txtName.text = name;
        txtValue.text = value.ToString("F1");

        float sin = Mathf.Sign(value);
        if (value == 0) sin = 0;

        Color statTextColor = Color.white;
        statTextColor = sin < 0 ? Color.red : Color.green;
        if (sin == 0) statTextColor = Color.white;

        if (!isColor) return;

        txtValue.color = statTextColor;
        txtName.color = statTextColor;
    }

    public float GetFontSize()
    {
        return txtName.fontSize;
    }

    public void SetFontSize(float minFontSize)
    {
        txtName.fontSizeMax = minFontSize;
        txtValue.fontSizeMax = minFontSize;
    }
}
