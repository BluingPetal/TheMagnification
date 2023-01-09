using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friends/EMP")]
public class EMPData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stats_Level1")]
    public float level1_hp;
    public int level1_cost;
    [Header("Stats_Level2")]
    public float level2_hp;
    [Header("Stats_Level3")]
    public float level3_hp;

    [Header("FX")]
    public ParticleSystem screenStopParticle;
    public ParticleSystem towerSkillParticle;
}
