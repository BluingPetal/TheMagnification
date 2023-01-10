using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/PoliceWithPole")]
public class PoliceWithPoleData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public int cost;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float attackRange;
    public float attackRoutine;
    public float attackPower;
    // 어느 스테이지에 나올지도 정하기
}
