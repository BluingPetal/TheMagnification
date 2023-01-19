using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SubsystemsImplementation;

public class WaveManager : SingleTon<WaveManager>
{
    [SerializeField]
    WaveData data;
    [SerializeField]
    private List<List<ObjectInfo>> Waves;

    [Header("Settings")]
    [SerializeField]
    private float routine;

    private Queue<GameObject> CurWaveObj;
    private List<GameObject> SpawnedObj;
    private int curWave;
    private Coroutine respawnCoroutine;
    private WaitForSeconds respawnTime;

    //Event
    [HideInInspector]
    public UnityEvent<int> OnWaveChanged;

    private void Awake()
    {
        curWave = 0;
        Waves = new List<List<ObjectInfo>>();
        CurWaveObj = new Queue<GameObject>();
        SpawnedObj = new List<GameObject>();
        respawnTime = new WaitForSeconds(routine);
    }

    private void Start()
    {
        Waves.Add(data.Wave1);
        Waves.Add(data.Wave2);
        Waves.Add(data.Wave3);
        Waves.Add(data.Wave4);
        Waves.Add(data.Wave5);
        Waves.Add(data.Wave6);
        Waves.Add(data.Wave7);
        Waves.Add(data.Wave8);
        Waves.Add(data.Wave9);
        Waves.Add(data.Wave10);
    }

    public void GoToNextWave()
    {
        WaveEnd();
        WaveStart();
    }

    public bool CheckEnemyStatus()
    {
        if (CurWaveObj.Count > 0)
            return false;

        for(int i=0; i<SpawnedObj.Count; i++)
        {
            WalkEnemy spawnedObj = SpawnedObj[i].GetComponent<WalkEnemy>();
            if(spawnedObj.HP > 0)
            {
                return false;
            }
        }
        return true;
    }
    private void WaveStart()
    {
        // TODO : wave가 모두 끝났을 때 엔딩 추가
        OnWaveChanged?.Invoke(curWave + 1);

        for (int i = 0; i < Waves[curWave].Count; i++)
        {
            for (int j = 0; j < Waves[curWave][i].num; j++)
                CurWaveObj.Enqueue(Waves[curWave][i].obj);
        }

        respawnCoroutine = StartCoroutine(Spawn());

        curWave++;
    }

    private IEnumerator Spawn()
    {
        while (CurWaveObj.Count > 0)
        {
            GameObject obj = CurWaveObj.Dequeue();
            if (obj != null)
            {
                // 왼쪽에 생성될 오브젝트들의 이름
                if (obj.name == "Enemy_PoliceWithPole" || obj.name == "Enemy_PoliceWithPistol"
                    || obj.name == "Enemy_Soldier" || obj.name == "Enemy_Buggy1" || obj.name == "Enemy_APC2")
                {
                    GameObject instantiatedObj = Instantiate(obj.gameObject, WayManager.Instance.WalkingWayPoints[0][0].position, Quaternion.identity);
                    instantiatedObj.GetComponent<WalkEnemy>().startWayNum = 0;
                    SpawnedObj.Add(instantiatedObj);
                }

                // 오른쪽에 생성될 오브젝트들의 이름
                if (obj.name == "Enemy_Knight" || obj.name == "Enemy_APC1"
                    || obj.name == "Enemy_APC3" || obj.name == "Enemy_Buggy2")
                {
                    GameObject instantiatedObj = Instantiate(obj.gameObject, WayManager.Instance.WalkingWayPoints[1][0].position, Quaternion.identity);
                    instantiatedObj.GetComponent<WalkEnemy>().startWayNum = 1;
                    SpawnedObj.Add(instantiatedObj);
                }

                yield return respawnTime;
            }
        }
    }

    private void WaveEnd()
    {
        // Wave가 끝났을 경우 스폰되었던 모든 적들을 제거
        for (int i = 0; i < SpawnedObj.Count; i++)
        {
            Destroy(SpawnedObj[i]);
        }
        SpawnedObj.Clear();
    }
}
