using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/WaveInfo")]
public class WaveData : ScriptableObject
{
    // wave마다 나올 enemy 정해주기
    [Header("Waves")]
     
    public List<ObjectInfo> Wave1;
    public List<ObjectInfo> Wave2;
    public List<ObjectInfo> Wave3;
    public List<ObjectInfo> Wave4;
    public List<ObjectInfo> Wave5;
    public List<ObjectInfo> Wave6;
    public List<ObjectInfo> Wave7;
    public List<ObjectInfo> Wave8;
    public List<ObjectInfo> Wave9;     
    public List<ObjectInfo> Wave10;
}

[Serializable]
public struct ObjectInfo
{
    public int num;
    public GameObject obj;
    public string type;
}