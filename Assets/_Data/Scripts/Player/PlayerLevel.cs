using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI txtLevel;

    private int requiredXP;
    private int currentXP;
    private int level;
    private int levelIsEarnedThisWave;

    private void Awake()
    {
        Candy.onCollected += GainExp;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= GainExp;
    }
    private void Start()
    {
        UpdateRequiredXP();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        xpBar.value = (float)currentXP / requiredXP;
        txtLevel.text = "lvl " + (level + 1);
    }

    private void UpdateRequiredXP()
    {
        requiredXP = (level + 1) * 5;
    }

    private void GainExp(Candy candy)
    {
        currentXP++;

        if (currentXP >= requiredXP)
        {
            LevelUp();
        }

        UpdateVisual();
    }

    private void LevelUp()
    {
        currentXP = 0;
        level++;
        levelIsEarnedThisWave++;
        UpdateRequiredXP();
    }

    public bool HasLevelUp()
    {
        if (levelIsEarnedThisWave > 0)
        {
            levelIsEarnedThisWave--;
            return true;
        }
        return false;
    }
}
