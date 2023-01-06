using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

[CreateAssetMenu(menuName = "Friends/MachineGun")]
public class MachineGunData : ScriptableObject
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
}
