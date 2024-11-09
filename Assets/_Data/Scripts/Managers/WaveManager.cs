
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WaveManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private Player player;
    [SerializeField] private int waveDuration;
    [SerializeField] private Wave[] waves;

    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    private List<float> localCounters = new List<float>();

    private WaveManagerUI waveManagerUI;

    private void Awake()
    {
        waveManagerUI = GetComponent<WaveManagerUI>();
    }

    private void Update()
    {
        if (!isTimerOn) return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();
        }
        else
        {
            StartWaveTransition();
        }

        waveManagerUI.UpdateTextTimer(Mathf.RoundToInt(waveDuration - timer).ToString());
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemy();
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length)
        {
            GameHandler.Instance.SetGameState(GameState.STAGECOMPLETE);
        }
        else
        {
            GameHandler.Instance.WaveCompleteCallBack();
        }

    }

    private void DefeatAllEnemy()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
        {
            enemy.PassAway();
        }
    }

    private void StartWave(int waveIndex)
    {
        waveManagerUI.UpdateTextWave("Wave " + (currentWaveIndex + 1));
        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments)
        {
            localCounters.Add(1);
        }

        timer = 0;
        isTimerOn = true;
    }

    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd) continue;

            float timeSinceSegmentStart = timer - tStart;
            float spawnDeplay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDeplay > localCounters[i])
            {
                Instantiate(segment.prefabs, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;
            }
        }

        timer += Time.deltaTime;
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6, 10);
        Vector2 targetPos = (Vector2)player.transform.position + offset;

        if (!GameHandler.Instance.UseInfiniteMap)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, -18, 18);
            targetPos.y = Mathf.Clamp(targetPos.y, -8, 8);
        }

        return targetPos;
    }

    public void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemy();
                break;

        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefabs;
}