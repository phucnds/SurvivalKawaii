using System;
using UnityEngine;


using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private UpgradeContainer[] upgradeContainers;
    [SerializeField] private Transform upgradeContainerParent;
    [SerializeField] private PlayerStatsManager playerStatsManager;

    [SerializeField] private ChestObjecContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;
    [SerializeField] private PlayerObjects playerObjects;

    private int chestCollected;

    public static WaveTransitionManager Instance;

    private void Awake()
    {
        Chest.onCollected += ChestCollectedCallback;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }

    public void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        chestContainerParent.Clear();
        if (chestCollected > 0)
        {
            ShowObject();
        }
        else
        {
            ConfigureUpgradeContainers();
        }
    }

    private void ShowObject()
    {
        chestCollected--;

        upgradeContainerParent.gameObject.SetActive(false);
        chestContainerParent.gameObject.SetActive(true);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjecContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObjectData);

        containerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData));
        containerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallBack(randomObjectData));
    }

    private void TakeButtonCallback(ObjectDataSO objectData)
    {
        playerObjects.AddObject(objectData);
        TryOpenChest();
    }

    private void RecycleButtonCallBack(ObjectDataSO objectData)
    {
        CurrencyManager.Instance.AddCurrency(objectData.RecyclePrice);
        TryOpenChest();
    }

    private void ConfigureUpgradeContainers()
    {
        upgradeContainerParent.gameObject.SetActive(true);
        chestContainerParent.gameObject.SetActive(false);

        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            string randomStatString = "";
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            Sprite spr = ResourcesManager.GetStatIcon(stat);

            upgradeContainers[i].Configure(spr, randomStatString, buttonString);
            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() =>
            {
                action?.Invoke();
                BonusSelectedCallBack();
            });
        }
    }

    private void BonusSelectedCallBack()
    {
        GameHandler.Instance.WaveCompleteCallBack();
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        switch (stat)
        {
            case Stat.Attack:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.AttackSpeed:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.CriticalChance:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.CriticalPercent:
                value = 0.1f;
                buttonString = "+" + value + "x";
                break;
            case Stat.MoveSpeed:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.MaxHealth:
                value = 10;
                buttonString = "+" + value;
                break;
            case Stat.Range:
                value = 10;
                buttonString = "+" + value;
                break;
            case Stat.HealthRecoverySpeed:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.Armor:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.Luck:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.Dodge:
                value = 10;
                buttonString = "+" + value + "%";
                break;
            case Stat.LifeSteal:
                value = 10;
                buttonString = "+" + value + "%";
                break;

            default:
                return () => Debug.Log("Invalid stat");

        }

        return () => playerStatsManager.AddPlayerStat(stat, value);
    }

    private void ChestCollectedCallback(Chest chest)
    {
        chestCollected++;
    }

    public bool HasChestCollected()
    {
        return chestCollected > 0;
    }
}
