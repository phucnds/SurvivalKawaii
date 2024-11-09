using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    [SerializeField] private PaletteSO palette;

    public static ColorHolder Instance;

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

    public static Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, Instance.palette.LevelColors.Length);
        return Instance.palette.LevelColors[level];
    }

    public static Color GetOutLineColor(int level)
    {
        level = Mathf.Clamp(level, 0, Instance.palette.LevelOutLineColors.Length);
        return Instance.palette.LevelOutLineColors[level];
    }

}
