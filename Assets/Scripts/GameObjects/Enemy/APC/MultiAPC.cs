using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �̻��ϰ� �Ѿ��� ���ÿ� ���� ������ APC
public class MultiAPC : Cars
{
    public APCData data;

    [SerializeField]
    private Transform missileTransform;
    [SerializeField]
    protected Shooter missileShooter;
    [SerializeField]
    protected Shooter shooter1;

    private GameObject missilePrefab;
    private float nearAttackTargetDistance;
    private bool bulletShooterActived;

    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;
        missilePrefab = data.missilePrefab;

        hp = data.hp;
        speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        nearAttackTargetDistance = data.nearAttackTargetDistance;
        attackPower = data.bulletPower;

        // shooter setting
        shooter.owner = this.gameObject;
        shooter1.owner = this.gameObject;
        missileShooter.kind = "Missile";
        missileShooter.owner = this.gameObject;

        // bullet �����ϴ� shooter�� Ȱ��ȭ �Ǿ��°� ����
        bulletShooterActived = false;
    }

    protected override void LookAtTarget()
    {
        if (target != null)
        {
            // target�� �ٶ󺸵��� ����
            if (bulletShooterActived)
                topTransform.LookAt(new Vector3(target.position.x, topTransform.position.y, target.position.z));
                
            missileTransform.LookAt(new Vector3(target.position.x, missileTransform.position.y, target.position.z));

            // Ÿ���� ���� ray �׸���
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
    }

    protected override void StateUpdate()
    {
        switch (state)
        {
            case CarState.Normal:
                // Ÿ���� ������ attack state�� �̵�
                if (target != null)
                {
                    // Ÿ�ٰ��� �Ÿ��� ����� bullet shooter�� Ȱ��ȭ ���� ����
                    Vector3 posDiffWithTarget1 = target.position - this.transform.position;
                    float sqrdistanceToTarget1 = posDiffWithTarget1.sqrMagnitude;
                    if (sqrdistanceToTarget1 < nearAttackTargetDistance)
                        bulletShooterActived = true;
                    else
                        bulletShooterActived = false;

                    state = CarState.Attack;
                    isMove = false;
                    Attack();
                    attackCoroutine = StartCoroutine(AttackDelay());
                    break;
                }
                break;

            case CarState.Attack:
                // Ÿ���� ������ normal state�� �̵�
                if (target == null)
                {
                    Debug.Log("nullTarget");
                    state = CarState.Normal;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("Normal");
                    break;
                }

                // Ÿ�ٰ��� �Ÿ��� ����� bullet shooter�� Ȱ��ȭ ���� ����
                Vector3 posDiffWithTarget2 = target.position - this.transform.position;
                float sqrdistanceToTarget2 = posDiffWithTarget2.sqrMagnitude;
                if (sqrdistanceToTarget2 < nearAttackTargetDistance)
                    bulletShooterActived = true;
                else
                    bulletShooterActived = false;

                break;
        }
    }

    protected override void Attack()
    {
        // �Ѿ� ������ shooter�� ����
        // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��

        // ���� bullet shooter Ȱ��ȭ ���ο� ���� �߻�Ǵ� Shooter�� ������ �ٸ�
        if (bulletShooterActived)
        {
            shooter.Shoot(target);
            shooter1.Shoot(target);
        }

        missileShooter.Shoot(target);

        animator.SetTrigger("Attack");
    }
}
