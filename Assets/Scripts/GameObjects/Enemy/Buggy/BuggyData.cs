using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Buggy")]
public class BuggyData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float attackRange;
    public float attackRoutine;
    public float attackPower;
    public float bulletSpeed;
    public float bulletScale;

    [Header("FX")]
    public ParticleSystem bulletParticle;
    // 어느 스테이지에 나올지도 정하기
}
