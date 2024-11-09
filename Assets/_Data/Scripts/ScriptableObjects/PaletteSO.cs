using UnityEngine;

[CreateAssetMenu(fileName = "PaletteSO", menuName = "DemonSlayer/PaletteSO", order = 2)]
public class PaletteSO : ScriptableObject
{
    [field: SerializeField] public Color[] LevelColors { get; private set; }
    [field: SerializeField] public Color[] LevelOutLineColors { get; private set; }
}