
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [SerializeField][Range(0, 100)] int cashDropChance = 20;
    [SerializeField][Range(0, 100)] int chestDropChance = 10;

    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;
    private ObjectPool<Chest> chestPool;

    private void Awake()
    {
        Enemy.onPassedAway += Enemy_onPassedAway;
        Candy.onCollected += OnReleaseCandy;
        Cash.onCollected += OnReleaseCash;
        Chest.onCollected += OnReleaseChest;
    }

    private void Start()
    {
        candyPool = new ObjectPool<Candy>(CandyCreateFunction, CandyActionOnGet, CandyActionOnRelease, CandyActionOnDestroy);
        cashPool = new ObjectPool<Cash>(CashCreateFunction, CashActionOnGet, CashActionOnRelease, CashActionOnDestroy);
        chestPool = new ObjectPool<Chest>(ChestCreateFunction, ChestActionOnGet, ChestActionOnRelease, ChestActionOnDestroy);
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= Enemy_onPassedAway;
        Candy.onCollected -= OnReleaseCandy;
        Cash.onCollected -= OnReleaseCash;
        Chest.onCollected -= OnReleaseChest;
    }

    private Candy CandyCreateFunction() => Instantiate(candyPrefab, transform);
    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);

    private Cash CashCreateFunction() => Instantiate(cashPrefab, transform);
    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);
    private void CashActionOnDestroy(Cash cash) => Destroy(cash.gameObject);

    private Chest ChestCreateFunction() => Instantiate(chestPrefab, transform);
    private void ChestActionOnGet(Chest chest) => chest.gameObject.SetActive(true);
    private void ChestActionOnRelease(Chest chest) => chest.gameObject.SetActive(false);
    private void ChestActionOnDestroy(Chest chest) => Destroy(chest.gameObject);

    private void Enemy_onPassedAway(Vector2 pos)
    {
        bool shouldSpawnCash = Random.Range(0, 100) < cashDropChance;

        DroppableCurrency droppable = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = pos;

        bool shouldSpawnChest = Random.Range(0, 100) < chestDropChance;
        if (!shouldSpawnChest) return;

        Chest chest = chestPool.Get();
        chest.transform.position = pos + Vector2.right;
    }

    private void OnReleaseCash(Cash cash) => cashPool.Release(cash);
    private void OnReleaseCandy(Candy candy) => candyPool.Release(candy);
    private void OnReleaseChest(Chest chest) => chestPool.Release(chest);
}
