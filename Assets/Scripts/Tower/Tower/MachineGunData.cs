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

    [Header("Stats_Level1")]
    public float level1_hp;
    public float level1_attackRange;
    public float level1_attackRoutine;
    public float level1_attackPower;
    public int level1_cost;
    [Header("Stats_Level2")]
    public float level2_hp;
    public float level2_attackRange;
    public float level2_attackRoutine;
    public float level2_attackPower;
    public int level2_cost;
    [Header("Stats_Level3")]
    public float level3_hp;
    public float level3_attackRange;
    public float level3_attackRoutine;
    public float level3_attackPower;
    public int level3_cost;

    [Header("BulletSetting")]
    public float bulletSpeed;
    public float bulletScale;

    [Header("FX")]
    public ParticleSystem bulletParticle;
}
