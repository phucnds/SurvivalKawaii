using UnityEngine;

[CreateAssetMenu(fileName = "StatIconData", menuName = "DemonSlayer/StatIconDataSO", order = 3)]
public class StatIconDataSO : ScriptableObject
{
    [field: SerializeField] public StatIcon[] StatIcons { get; private set; }
}

[System.Serializable]
public struct StatIcon
{
    public Stat stat;
    public Sprite sprite;
}