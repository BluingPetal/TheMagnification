using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingleTon<GameManager>
{
    [HideInInspector]
    public UnityEvent OnWaveChanged;
    [HideInInspector]
    public UnityEvent<int> OnWaveTimeChanged;
    [SerializeField]
    private int waveTime;
    private int curWaveTime;

    public int WaveTime
    { get { return curWaveTime; } private set { curWaveTime = value; OnWaveTimeChanged?.Invoke(curWaveTime); } }

    private void Start()
    {
        WaveTime = 10;
        StartCoroutine(GoToNextWave());
    }

    public void CheckIfWaveDone()
    {
        if(WaveManager.Instance.CheckEnemyStatus())
        {
            // ���� ��ƼŬ �ý��� �ѷ��ְ� 10�� ��ٸ� �Ŀ� ���� wave�� ����
            StartCoroutine(GoToNextWave());
        }
    }

    private IEnumerator GoToNextWave()
    {
        OnWaveChanged?.Invoke();
        for (int i = 0; i < waveTime; i++)
        {
            yield return new WaitForSeconds(1f);
            WaveTime--;
        }

        WaveManager.Instance.GoToNextWave();
        WaveTime = 10;
        // after Ȱ��ȭ, before ��Ȱ��ȭ 
        OnWaveChanged?.Invoke();
    }

    public void OpenShop()
    {
        // ��ư�� ������ ��� ����â ����
        GameObject.Find("ShopCanvas").transform.GetChild(0).gameObject?.SetActive(true);
    }

    public void CloseShop()
    {
        // �ݱ� ��ư�� ������ ��� ����â �ݱ�
        GameObject.Find("ShopCanvas").transform.GetChild(0).gameObject?.SetActive(false);
    }
}