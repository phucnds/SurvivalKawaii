using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool IsSFXOn { get; private set; }
    public bool IsBGMOn { get; private set; }

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

        SettingsManager.onSfxStateChange += SFXStateChangeCallback;
        SettingsManager.onBgmStateChange += BGMStateChangeCallback;
    }

    private void OnDestroy()
    {
        SettingsManager.onSfxStateChange -= SFXStateChangeCallback;
        SettingsManager.onBgmStateChange -= BGMStateChangeCallback;
    }

    private void BGMStateChangeCallback(bool bgmState)
    {
        IsBGMOn = bgmState;
    }

    private void SFXStateChangeCallback(bool sfxState)
    {
        IsSFXOn = sfxState;
    }
}
