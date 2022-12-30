using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Buggy2Data))]
public class Buggy2 : Cars
{
    [SerializeField]
    private Buggy2Data data;
    
    protected override void Awake()
    {
        base.Awake();
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;

        hp = data.hp;
        speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;
    }
}

