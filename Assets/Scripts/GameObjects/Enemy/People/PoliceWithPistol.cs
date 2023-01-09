using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceWithPistol : HumanoidEnemies
{
    public PoliceWithPistolData data;

    private GameObject bulletPrefab;

    [SerializeField]
    protected Shooter shooter;

    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;

        maxHp = hp = data.hp;
        maxSpeed = speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;

        // shooter setting
        shooter.owner = this.gameObject;
    }
    protected override void Attack()
    {
        if (target != null)
        {
            if (state == HumanoidEnemyState.NearAttack)
            {
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
