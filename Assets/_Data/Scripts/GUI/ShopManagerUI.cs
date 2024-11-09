using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform playerStatPanel;
    [SerializeField] private RectTransform closePlayerStat;

    [SerializeField] private RectTransform playerInventoryPanel;
    [SerializeField] private RectTransform closeInventory;

    [SerializeField] private RectTransform itemInfoPanel;


    [Space(15)]
    [HorizontalLine]
    [SerializeField] private RectTransform s;

    private Vector2 playerStatsOpenedPos;
    private Vector2 playerStatsClosedPos;

    private Vector2 playerInventoryOpenedPos;
    private Vector2 playerInventoryClosedPos;

    private Vector2 itemInfoPanelOpenedPos;
    private Vector2 itemInfoPanelClosedPos;

    private IEnumerator Start()
    {
        yield return null;
        ConfigurePlayerStatPanel();
        ConfigurePlayerInventoryPanel();
        ConfigureItemInfoPanelPanel();
    }

    private void ConfigurePlayerStatPanel()
    {
        float width = Screen.width / (4 * playerStatPanel.lossyScale.x);
        playerStatPanel.offsetMax = playerStatPanel.offsetMax.With(x: width);

        playerStatsOpenedPos = playerStatPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos + Vector2.left * width;

        playerStatPanel.anchoredPosition = playerStatsClosedPos;
        HidePlayerStat();
    }

    [NaughtyAttributes.Button]
    public void ShowPlayerStat()
    {
        playerStatPanel.gameObject.SetActive(true);
        closePlayerStat.gameObject.SetActive(true);
        closePlayerStat.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerStatPanel);
        LeanTween.move(playerStatPanel, playerStatsOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(closePlayerStat);
        LeanTween.alpha(closePlayerStat, .8f, .5f).setRecursive(false);
    }

    [NaughtyAttributes.Button]
    public void HidePlayerStat()
    {
        closePlayerStat.GetComponent<Image>().raycastTarget = false;
        LeanTween.cancel(playerStatPanel);
        LeanTween.move(playerStatPanel, playerStatsClosedPos, .5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => playerStatPanel.gameObject.SetActive(false));

        LeanTween.cancel(closePlayerStat);
        LeanTween.alpha(closePlayerStat, 0, .5f).setRecursive(false).setOnComplete(() => closePlayerStat.gameObject.SetActive(false));
    }

    private void ConfigurePlayerInventoryPanel()
    {
        float width = Screen.width / (4 * playerInventoryPanel.lossyScale.x);
        playerInventoryPanel.offsetMin = playerInventoryPanel.offsetMin.With(x: -width);

        playerInventoryOpenedPos = playerInventoryPanel.anchoredPosition;
        playerInventoryClosedPos = playerStatsOpenedPos + Vector2.right * width;

        playerInventoryPanel.anchoredPosition = playerInventoryClosedPos;
        HidePlayerInventory(false);
    }

    [NaughtyAttributes.Button]
    public void ShowPlayerInventory()
    {
        playerInventoryPanel.gameObject.SetActive(true);
        closeInventory.gameObject.SetActive(true);
        closeInventory.GetComponent<Image>().raycastTarget = true;

        LeanTween.cancel(playerInventoryPanel);
        LeanTween.move(playerInventoryPanel, playerInventoryOpenedPos, .5f).setEase(LeanTweenType.easeInCubic);

        LeanTween.cancel(closeInventory);
        LeanTween.alpha(closeInventory, .8f, .5f).setRecursive(false);
    }

    [NaughtyAttributes.Button]
    public void HidePlayerInventory(bool hideItemInfo = true)
    {
        closeInventory.GetComponent<Image>().raycastTarget = false;
        LeanTween.cancel(playerInventoryPanel);
        LeanTween.move(playerInventoryPanel, playerInventoryClosedPos, .5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => playerInventoryPanel.gameObject.SetActive(false));

        LeanTween.cancel(closeInventory);
        LeanTween.alpha(closeInventory, 0, .5f).setRecursive(false).setOnComplete(() => closeInventory.gameObject.SetActive(false));

        if (hideItemInfo)
            HideItemInfo();
    }

    private void ConfigureItemInfoPanelPanel()
    {
        float height = Screen.height / (2 * itemInfoPanel.lossyScale.y);
        itemInfoPanel.offsetMax = itemInfoPanel.offsetMax.With(y: height);

        itemInfoPanelOpenedPos = itemInfoPanel.anchoredPosition;
        itemInfoPanelClosedPos = itemInfoPanelOpenedPos + Vector2.down * height;

        itemInfoPanel.anchoredPosition = itemInfoPanelClosedPos;

    }

    [NaughtyAttributes.Button]
    public void ShowItemInfo()
    {
        itemInfoPanel.gameObject.SetActive(true);
        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoPanelOpenedPos, .3f).setEase(LeanTweenType.easeOutCubic);
    }

    [NaughtyAttributes.Button]
    public void HideItemInfo()
    {
        itemInfoPanel.LeanCancel();
        itemInfoPanel.LeanMove((Vector3)itemInfoPanelClosedPos, .3f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => itemInfoPanel.gameObject.SetActive(false));
    }
}
