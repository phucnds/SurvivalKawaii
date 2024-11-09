using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private CharacterDataSO playerData;

    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> objectAddends = new Dictionary<Stat, float>();

    private void Awake()
    {
        CharacterSelectionManager.onCharacterSelected += CharacterSelectedCallback;

        playerStats = playerData.BaseStat;

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
        {
            addends.Add(kvp.Key, 0);
            objectAddends.Add(kvp.Key, 0);
        }
    }

    private void OnDestroy()
    {
        CharacterSelectionManager.onCharacterSelected -= CharacterSelectedCallback;
    }

    private void Start()
    {
        UpdatePlayerStats();
    }

    public void AddPlayerStat(Stat stat, float value)
    {
        if (addends.ContainsKey(stat))
        {
            addends[stat] += value;
        }
        else
        {
            Debug.LogWarning($"The key {stat} has not been found");
        }

        UpdatePlayerStats();
    }

    public float GetStatValue(Stat stat) => playerStats[stat] + addends[stat] + objectAddends[stat];

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDepnedency> playerStatsDepnedencies = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IPlayerStatsDepnedency>();

        foreach (IPlayerStatsDepnedency depnedency in playerStatsDepnedencies)
        {
            depnedency.UpdateStats(this);
        }
    }

    public void AddObject(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            if (objectAddends.ContainsKey(kvp.Key))
            {
                objectAddends[kvp.Key] += kvp.Value;
            }
        }

        UpdatePlayerStats();
    }

    public void RemoveObjectStat(Dictionary<Stat, float> objectStats)
    {
        foreach (KeyValuePair<Stat, float> kvp in objectStats)
        {
            if (objectAddends.ContainsKey(kvp.Key))
            {
                objectAddends[kvp.Key] -= kvp.Value;
            }
        }

        UpdatePlayerStats();
    }

    private void CharacterSelectedCallback(CharacterDataSO data)
    {
        playerData = data;
        playerStats = data.BaseStat;

        UpdatePlayerStats();
    }

}
