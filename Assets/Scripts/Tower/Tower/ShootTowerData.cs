using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friends/ShootTower")]
public class ShootTowerData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public Sprite icon;
    public GameObject prefab;
    public GameObject bulletPrefab;

    [Header("Stats_Level1")]
    public float level1_attackRange;
    public float level1_attackRoutine;
    public float level1_attackPower;
    public int level1_cost;
    public int level1_continuousShot;
    [Header("Stats_Level2")]
    public float level2_attackRange;
    public float level2_attackRoutine;
    public float level2_attackPower;
    public int level2_cost;
    public int level2_continuousShot;
    [Header("Stats_Level3")]
    public float level3_attackRange;
    public float level3_attackRoutine;
    public float level3_attackPower;
    public int level3_cost;
    public int level3_continuousShot;

    [Header("BulletSetting")]
    public float bulletSpeed;
    public float bulletScale;

    [Header("FX")]
    public ParticleSystem bulletParticle;
    public ParticleSystem bulletExplosion;
}
