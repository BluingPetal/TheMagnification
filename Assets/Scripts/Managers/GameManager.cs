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
            // ���� ��ƼŬ �ý��� �ѷ��ְ� 5�� ��ٸ� �Ŀ� ���� wave�� ����
            StartCoroutine(GoToNextWave());
        }
    }

    private IEnumerator GoToNextWave()
    {
        yield return new WaitForSeconds(5f);
        WaveManager.Instance.GoToNextWave();
    }
}