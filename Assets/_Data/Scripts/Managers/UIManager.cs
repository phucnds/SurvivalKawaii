using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject restartConfirmationPanel;
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject settingsPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[] {
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            gameOverPanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel
        });

        GameHandler.onGamePaused += GamePausedCallback;
        GameHandler.onGameResumed += GameResumedCallback;

        pausePanel.SetActive(false);
        characterSelectionPanel.SetActive(false);
        HideRestartConfirmationPanel();
        HideSettingPanel();
    }

    private void OnDestroy()
    {
        GameHandler.onGamePaused -= GamePausedCallback;
        GameHandler.onGameResumed -= GameResumedCallback;
    }

    public void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;

            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;

            case GameState.GAME:
                ShowPanel(gamePanel);
                break;

            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
        }
    }

    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject item in panels)
        {
            item.SetActive(item == panel);
        }
    }

    private void GamePausedCallback() => pausePanel.SetActive(true);
    private void GameResumedCallback() => pausePanel.SetActive(false);

    public void ShowRestartConfirmationPanel() => restartConfirmationPanel.SetActive(true);
    public void HideRestartConfirmationPanel() => restartConfirmationPanel.SetActive(false);

    public void ShowCharacterSelectionPanel() => characterSelectionPanel.SetActive(true);
    public void HideCharacterSelectionPanel() => characterSelectionPanel.SetActive(false);

    public void ShowSettingsPanel() => settingsPanel.SetActive(true);
    public void HideSettingPanel() => settingsPanel.SetActive(false);
}
