using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }

    private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    private void Start()
    {
        foreach (ObjectDataSO objectData in Objects)
        {
            playerStatsManager.AddObject(objectData.BaseStat);
        }
    }

    public void AddObject(ObjectDataSO objectData)
    {
        Objects.Add(objectData);
        playerStatsManager.AddObject(objectData.BaseStat);
    }

    public void RecycleObject(ObjectDataSO objectData)
    {
        Objects.Remove(objectData);
        CurrencyManager.Instance.AddCurrency(objectData.RecyclePrice);
        playerStatsManager.RemoveObjectStat(objectData.BaseStat);
    }
}
