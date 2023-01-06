using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buggy : Cars
{
    public BuggyData data;

    private void Awake()
    {
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

        // shooter setting
        shooter.owner = this.gameObject;
    }

    protected override void Attack()
    {
        // 총알 생성은 shooter가 진행
        // 타겟을 바로 공격하면 bullet과 싱크가 맞지 않으므로, bullet script에서 처리
        if (target != null)
        {
            shooter.Shoot(target);
            animator.SetTrigger("Attack");
        }
    }
}

