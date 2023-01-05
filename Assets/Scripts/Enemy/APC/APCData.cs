using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/APC")]
public class APCData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float attackRange;
    public float attackRoutine;
    public float nearAttackTargetDistance;

    [Header("Bullets")]
    public float bulletSpeed;
    public float bulletScale;
    public float bulletPower;
    public float missileSpeed;
    public float missileScale;
    public float missilePower;
    // 어느 스테이지에 나올지도 정하기
}
