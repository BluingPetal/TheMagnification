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

    private bool shopOpened;
    private bool inventoryOpened;

    public int WaveTime
    { get { return curWaveTime; } private set { curWaveTime = value; OnWaveTimeChanged?.Invoke(curWaveTime); } }

    private void Start()
    {
        WaveTime = 10;
        shopOpened = false;
        inventoryOpened = false;
        StartCoroutine(GoToNextWave());
    }

    public void CheckIfWaveDone()
    {
        if (WaveManager.Instance.CheckEnemyStatus())
        {
            // 성공 파티클 시스템 뿌려주고 10초 기다린 후에 다음 wave로 가기
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
        // after 활성화, before 비활성화 
        OnWaveChanged?.Invoke();
    }

    public void OpenShop()
    {
        // 버튼을 눌렀을 경우 상점창 열기
        if (!inventoryOpened)
        {
            GameObject.Find("ShopCanvas").transform.GetChild(0).gameObject?.SetActive(true);
            shopOpened = true;
        }
    }

    public void CloseShop()
    {
        // 닫기 버튼을 눌렀을 경우 상점창 닫기
        if (!inventoryOpened)
        {
            GameObject.Find("ShopCanvas").transform.GetChild(0).gameObject?.SetActive(false);
            shopOpened = false;
        }
    }

    public void OpenCloseInventory()
    {
        // 버튼을 눌렀을 경우 인벤토리창 열기
        if (!shopOpened)
        {
            GameObject inventory = GameObject.Find("InventoryCanvas").transform.GetChild(0).gameObject;
            inventory?.SetActive(!inventory.activeSelf);
            inventoryOpened = inventory.activeSelf;
        }
    }
}