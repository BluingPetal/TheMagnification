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
        // �Ѿ� ������ shooter�� ����
        // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��
        if (target != null)
        {
            shooter.Shoot(target);
            animator.SetTrigger("Attack");
        }
    }
}

