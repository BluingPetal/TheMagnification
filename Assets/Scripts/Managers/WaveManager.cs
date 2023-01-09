using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class WaveManager : SingleTon<WaveManager>
{
    // wave마다 나올 enemy 정해주기
    [Header("Waves")]
    [SerializeField]
    private List<ObjectInfo> Wave1;
    [SerializeField]
    private List<ObjectInfo> Wave2;

    [Header("Settings")]
    [SerializeField]
    private float routine;
    [SerializeField]
    private float waveStartRoutine;

    private Queue<GameObject> CurWaveObj;
    private List<GameObject> SpawnedObj;
    private int curWave;
    private Coroutine respawnCoroutine;
    private Coroutine waveStartCoroutine;
    private WaitForSeconds respawnTime;
    private WaitForSeconds waveStartTime;

    private void Awake()
    {
        curWave = 0;
        CurWaveObj = new Queue<GameObject>();
        SpawnedObj = new List<GameObject>();
        respawnTime = new WaitForSeconds(routine);
        waveStartTime = new WaitForSeconds(waveStartRoutine);
    }

    private void Start()
    {
        waveStartCoroutine = StartCoroutine(WaveStart());
    }

    private IEnumerator WaveStart()
    {
        curWave++;
        yield return waveStartTime;

        switch(curWave)
        {
            case 1:
                for (int i = 0; i < Wave1.Count; i++)
                {
                    for (int j = 0; j < Wave1[i].num; j++)
                        CurWaveObj.Enqueue(Wave1[i].obj);
                }
                break;
            case 2:
                for (int i = 0; i < Wave2.Count; i++)
                {
                    for (int j = 0; j < Wave2[i].num; j++)
                        CurWaveObj.Enqueue(Wave2[i].obj);
                }
                break;
        }

        respawnCoroutine = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(CurWaveObj.Count > 0)
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
                if(obj.name == "Enemy_Knight" || obj.name == "Enemy_APC1"
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

    public void GoToNextWave()
    {
        //if(CheckEnemyStatus())
        //{
            WaveEnd();
            waveStartCoroutine = StartCoroutine(WaveStart());
        //}
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

    private void WaveEnd()
    {
        // Wave가 끝났을 경우 스폰되었던 모든 적들을 제거
        for (int i = 0; i < SpawnedObj.Count; i++)
        {
            Destroy(SpawnedObj[i]);
        }
        SpawnedObj.Clear();
    }

    [Serializable]
    public struct ObjectInfo
    {
        public int num;
        public GameObject obj;
    }
}
