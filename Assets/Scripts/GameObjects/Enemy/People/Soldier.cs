using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : HumanoidEnemies
{
    public SoldierData data;

    private GameObject bulletPrefab;
    private int curNearAttack;

    [SerializeField]
    protected Shooter shooter;


    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;
        cost = data.cost;

        maxHp = hp = data.hp;
        maxSpeed = speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;

        // shooter setting
        shooter.owner = this.gameObject;
        curNearAttack = 0;
    }
    protected override void Attack()
    {
        if (target != null)
        {
            if (state == HumanoidEnemyState.NearAttack)
            {
                curNearAttack++;
                animator.SetInteger("CurNearAttack", curNearAttack % 2);

                // NearAttack �ִϸ��̼ǿ��� Animation Event�� ���� target���� �������� ������ ����
                animator.SetTrigger("NearAttack");
            }
            else if (state == HumanoidEnemyState.Attack)
            {
                // Attack �ִϸ��̼ǿ��� Animation Event�� ���� shooter�� ���� ��� ����
                animator.SetTrigger("Attack");
            }
        }
    }

    public void Shoot()
    {
        // �Ѿ� ������ shooter�� ����
        // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��
        shooter.Shoot(target);
    }

    public void GiveDamage()
    {
        // ���� ���ݿ��� ȣ��� �Լ�
        if (target != null)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            damageable?.TakeDamage(attackPower);
        }
    }
}
