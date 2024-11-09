using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsDisplay : MonoBehaviour, IPlayerStatsDepnedency
{
    [SerializeField] private Transform playerStatContainerParent;

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        int index = 0;

        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            StatContainer statContainer = playerStatContainerParent.GetChild(index).GetComponent<StatContainer>();

            Sprite statIcon = ResourcesManager.GetStatIcon(stat);
            float statValue = playerStatsManager.GetStatValue(stat);
            statContainer.Configure(statIcon, Enums.FormatStatName(stat), statValue, true);
            statContainer.gameObject.SetActive(true);

            index++;
        }

        for (int i = index; i < playerStatContainerParent.childCount; i++)
        {
            playerStatContainerParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
