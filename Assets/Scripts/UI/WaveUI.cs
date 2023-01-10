using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI waveText;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        GameManager.Instance.OnWaveChanged.AddListener(WaveChanged);
        GameManager.Instance.OnWaveTimeChanged.AddListener(ChangeTime);
        WaveManager.Instance.OnWaveChanged.AddListener(ChangeWave);
    }

    private void ChangeTime(int time)
    {
        timeText.text = time.ToString();
    }

    private void ChangeWave(int wave)
    {
        waveText.text = wave.ToString();
    }

    private void WaveChanged()
    {
        // before wave
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        // after wave
        transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
    }
}
