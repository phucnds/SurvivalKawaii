using System;
using System.Collections;
using System.Collections.Generic;
using Tabsil.Sijil;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour, IWantToBeSaved
{

    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [SerializeField] private Button sfxButton;
    [SerializeField] private Button bgmButton;
    [SerializeField] private Button policyButton;
    [SerializeField] private Button askButton;
    [SerializeField] private GameObject creditsPanel;

    private bool sfxState;
    private bool bgmState;

    private const string sfxStateKey = "sfxStateKey";
    private const string bgmStateKey = "bgmStateKey";

    public static Action<bool> onSfxStateChange;
    public static Action<bool> onBgmStateChange;

    private void Awake()
    {
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SFXButtonCallback);

        bgmButton.onClick.RemoveAllListeners();
        bgmButton.onClick.AddListener(BGMButtonCallback);

        policyButton.onClick.RemoveAllListeners();
        policyButton.onClick.AddListener(PrivacyPolicyButtonCallback);

        askButton.onClick.RemoveAllListeners();
        askButton.onClick.AddListener(AskButtonCallback);

        HideCreditsPanel();
    }

    private void Start()
    {
        onSfxStateChange?.Invoke(sfxState);
        onBgmStateChange?.Invoke(bgmState);
    }

    private void SFXButtonCallback()
    {
        sfxState = !sfxState;
        UpdateSFXButtonVisuals();
        Save();
    }

    private void BGMButtonCallback()
    {
        bgmState = !bgmState;
        UpdateBGMButtonVisuals();
        Save();
    }

    private void UpdateSFXButtonVisuals()
    {
        if (sfxState)
        {
            sfxButton.image.color = onColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "On";
        }
        else
        {
            sfxButton.image.color = offColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "Off";
        }

        onSfxStateChange?.Invoke(sfxState);
    }
    private void UpdateBGMButtonVisuals()
    {
        if (bgmState)
        {
            bgmButton.image.color = onColor;
            bgmButton.GetComponentInChildren<TextMeshProUGUI>().text = "On";
        }
        else
        {
            bgmButton.image.color = offColor;
            bgmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Off";
        }

        onBgmStateChange?.Invoke(bgmState);
    }

    public void Load()
    {
        bgmState = true;
        sfxState = true;

        if (Sijil.TryLoad(this, bgmStateKey, out object bgmStateObject))
        {
            bgmState = (bool)bgmStateObject;
        }

        if (Sijil.TryLoad(this, sfxStateKey, out object sfxStateObject))
        {
            sfxState = (bool)sfxStateObject;
        }

        UpdateBGMButtonVisuals();
        UpdateSFXButtonVisuals();

    }

    public void ShowCreditsPanel() => creditsPanel.SetActive(true);
    public void HideCreditsPanel() => creditsPanel.SetActive(false);

    private void PrivacyPolicyButtonCallback()
    {
        Application.OpenURL("https://www.youtube.com/");
    }

    private void AskButtonCallback()
    {
        string email = "nduongphuc@gmail.com";
        string subject = MyEscapeURL("Demonslayer");
        string body = MyEscapeURL("I need help with this ...");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private string MyEscapeURL(string s)
    {
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    public void Save()
    {
        Sijil.Save(this, sfxStateKey, sfxState);
        Sijil.Save(this, bgmStateKey, bgmState);
    }
}
