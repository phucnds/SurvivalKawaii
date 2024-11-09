using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [field: SerializeField] public bool UseInfiniteMap { get; private set; }

    public static Action onGamePaused;
    public static Action onGameResumed;
    public static GameHandler Instance;

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

    private void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    public void SetGameState(GameState state)
    {
        IEnumerable<IGameStateListener> gameStateListeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach (IGameStateListener gameStateListener in gameStateListeners)
        {
            gameStateListener.GameStateChangeCallback(state);
        }
    }

    public void StartGame() => SetGameState(GameState.GAME);
    public void WeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void StartShop() => SetGameState(GameState.SHOP);

    public void ManageGameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void WaveCompleteCallBack()
    {
        if (Player.Instance.HasLevelUp() || WaveTransitionManager.Instance.HasChestCollected())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public void PauseButtonCallback()
    {
        Time.timeScale = 0;
        onGamePaused?.Invoke();
    }

    public void ResumeButtonCallback()
    {
        Time.timeScale = 1;
        onGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        ManageGameOver();
    }
}

public interface IGameStateListener
{
    void GameStateChangeCallback(GameState gameState);
}

