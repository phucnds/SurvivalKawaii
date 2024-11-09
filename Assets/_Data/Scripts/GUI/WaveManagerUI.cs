using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWave;
    [SerializeField] private TextMeshProUGUI txtTimer;

    public void UpdateTextWave(string waveString) => txtWave.text = waveString;
    public void UpdateTextTimer(string timerString) => txtTimer.text = timerString;
}
