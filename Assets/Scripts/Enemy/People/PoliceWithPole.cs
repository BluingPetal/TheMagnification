using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceWithPole : HumanoidEnemies
{
    [SerializeField]
    private PoliceWithPoleData data;

    protected override void Awake()
    {
        base.Awake();
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;

        hp = data.hp;
        speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;
    }

    protected void GiveDamage()
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(attackPower);
    }
}
