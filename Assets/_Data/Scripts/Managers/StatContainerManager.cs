using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    [SerializeField] private StatContainer statContainerPrefab;

    public static StatContainerManager Instance;

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
    }

    private void GenerateContainer(Dictionary<Stat, float> statDictionary, Transform statContainersParent)
    {
        List<StatContainer> statContainers = new List<StatContainer>();

        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer containerInstance = Instantiate(statContainerPrefab, statContainersParent);
            statContainers.Add(containerInstance);
            containerInstance.Configure(ResourcesManager.GetStatIcon(kvp.Key), Enums.FormatStatName(kvp.Key), kvp.Value);
        }

        LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeTexts(statContainers));
    }

    private void ResizeTexts(List<StatContainer> statContainers)
    {
        float minFontSize = 5000;
        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();
            if (fontSize < minFontSize)
            {
                minFontSize = fontSize;
            }
        }

        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize);
        }
    }

    public static void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform statContainersParent)
    {
        statContainersParent.Clear();
        Instance.GenerateContainer(statDictionary, statContainersParent);
    }
}
