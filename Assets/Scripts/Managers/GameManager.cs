using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public void CheckIfWaveDone()
    {
        if(WaveManager.Instance.CheckEnemyStatus())
        {
            // 성공 파티클 시스템 뿌려주고 5초 기다린 후에 다음 wave로 가기
            StartCoroutine(GoToNextWave());
        }
    }

    private IEnumerator GoToNextWave()
    {
        yield return new WaitForSeconds(5f);
        WaveManager.Instance.GoToNextWave();
    }
}