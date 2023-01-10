using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Soldier")]
public class SoldierData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public GameObject bulletPrefab;
    public int cost;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float attackRange;
    public float attackRoutine;
    public float attackPower;

    [Header("BulletSetting")]
    public float bulletSpeed;
    public float bulletScale;

    [Header("FX")]
    public ParticleSystem bulletParticle;
    // 어느 스테이지에 나올지도 정하기
}
