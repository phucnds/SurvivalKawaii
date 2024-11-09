using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [SerializeField] private Transform containerParent;
    [SerializeField] private WeaponSelectionContainer weaponSelectionPrefab;
    [SerializeField] private WeaponDataSO[] starterWeapons;
    [SerializeField] private PlayerWeapons playerWeapons;

    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:

                if (selectedWeapon == null) return;
                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;

            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Configure()
    {
        containerParent.Clear();

        for (int i = 0; i < 3; i++)
        {
            GenerateWeaponContainer();
        }
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponSelectionPrefab, containerParent);
        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 5);


        containerInstance.Configure(weaponData, level);
        containerInstance.Button.onClick.RemoveAllListeners();
        containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallBack(containerInstance, weaponData, level));
    }

    private void WeaponSelectedCallBack(WeaponSelectionContainer containerIns, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;
        foreach (WeaponSelectionContainer container in containerParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerIns)
            {
                container.Select();
            }
            else
            {
                container.Deselect();
            }
        }
    }
}
