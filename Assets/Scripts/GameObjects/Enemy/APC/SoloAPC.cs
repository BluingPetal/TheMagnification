using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Weapon { Bullet, Missile }
public class SoloAPC : Cars
{
    public APCData data;

    [SerializeField]
    private Transform missileTransform;
    [SerializeField]
    protected Shooter missileShooter;

    private Weapon curWeapon;
    private GameObject missilePrefab;
    private float nearAttackTargetDistance;

    private bool changedWeapon;

    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;
        bulletPrefab = data.bulletPrefab;
        missilePrefab = data.missilePrefab;
        cost = data.cost;

        maxHp = hp = data.hp;
        maxSpeed = speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        nearAttackTargetDistance = data.nearAttackTargetDistance;
        attackPower = data.bulletPower;

        // shooter setting
        shooter.owner = this.gameObject;
        missileShooter.kind = "Missile";
        missileShooter.owner = this.gameObject;

        // ���� weapon : �̻���
        curWeapon = Weapon.Missile;
        changedWeapon = true;
        animator.SetInteger("CurWeapon", (int)curWeapon);
        animator.SetBool("ChangedWeapon", changedWeapon);
    }

    protected override void LookAtTarget()
    {
        if (target != null)
        {
            // target�� �ٶ󺸵��� ����
            if (curWeapon == Weapon.Bullet)
                topTransform.LookAt(target);
            else
                missileTransform.LookAt(target);

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
                    Vector3 posDiffWithTarget1 = target.position - this.transform.position;
                    float sqrdistanceToTarget1 = posDiffWithTarget1.sqrMagnitude;
                    if (sqrdistanceToTarget1 < nearAttackTargetDistance)
                    {
                        if (curWeapon == Weapon.Bullet)
                        {
                            state = CarState.Attack;
                            animator.SetBool("Normal", false);
                            Attack();
                            attackCoroutine = StartCoroutine(AttackDelay());
                        }
                        else
                        {
                            state = CarState.ChangeWeapon;
                            animator.SetBool("Normal", false);
                            changedWeapon = false;
                            animator.SetBool("ChangedWeapon", changedWeapon);
                        }
                        isMove = false;
                        break;
                    }
                    else
                    {
                        if (curWeapon == Weapon.Missile)
                        {
                            state = CarState.Attack;
                            animator.SetBool("Normal", false);
                            Attack();
                            attackCoroutine = StartCoroutine(AttackDelay());
                        }
                        else
                        {
                            state = CarState.ChangeWeapon;
                            animator.SetBool("Normal", false);
                            changedWeapon = false;
                            animator.SetBool("ChangedWeapon", changedWeapon);
                        }
                        isMove = false;
                        break;
                    }
                }
                break;

            case CarState.ChangeWeapon:
                if (curWeapon == Weapon.Bullet)
                {
                    if (!changedWeapon)
                    {
                        animator.SetInteger("CurWeapon", (int)curWeapon); // -> �ִϸ����� �ٲ��ִ� Ʈ���� (�Ѿ˿��� �̻��Ϸ�)
                        curWeapon = Weapon.Missile;
                        changedWeapon = true;
                        break;
                    }
                }
                else
                {
                    if (!changedWeapon)
                    {
                        animator.SetInteger("CurWeapon", (int)Weapon.Missile); // -> �ִϸ����� �ٲ��ִ� Ʈ���� (�̻��Ͽ��� �Ѿ˷�)
                        curWeapon = Weapon.Bullet;
                        changedWeapon = true;
                        break;
                    }
                }
                break;

            case CarState.Attack:
                // Ÿ���� ������ normal state�� �̵�
                if (target == null)
                {
                    state = CarState.Normal;
                    isMove = true;
                    changedWeapon = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetBool("Normal", true);
                    break;
                }
                Vector3 posDiffWithTarget2 = target.position - this.transform.position;
                float sqrdistanceToTarget2 = posDiffWithTarget2.sqrMagnitude;
                if (sqrdistanceToTarget2 < nearAttackTargetDistance)
                {
                    if (curWeapon == Weapon.Missile)
                    {
                        state = CarState.ChangeWeapon;
                        changedWeapon = false;
                        animator.SetBool("ChangedWeapon", changedWeapon);
                        StopCoroutine(attackCoroutine);
                        break;
                    }
                }
                else
                {
                    if (curWeapon == Weapon.Bullet)
                    {
                        state = CarState.ChangeWeapon;
                        changedWeapon = false;
                        animator.SetBool("ChangedWeapon", changedWeapon);
                        StopCoroutine(attackCoroutine);
                        break;
                    }
                }
                break;
        }
    }

    public void FinishedChangeWeapon()
    {
        if (target == null)
        {
            state = CarState.Normal;
            isMove = true;
            animator.SetBool("Normal", true);
            return;
        }
        state = CarState.Attack;
        animator.SetInteger("CurWeapon", (int)curWeapon);
        animator.SetBool("ChangedWeapon", changedWeapon);
        Attack();
        attackCoroutine = StartCoroutine(AttackDelay());
    }

    protected override void Attack()
    {
        // �Ѿ� ������ shooter�� ����
        // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��
        //animator.SetInteger("CurWeapon", (int)curWeapon);
        // ���� weapon�� ���� �߻�Ǵ� Shooter�� �ٸ�
        if (curWeapon == Weapon.Bullet)
            shooter.Shoot(target);
        else
            missileShooter.Shoot(target);

        animator.SetTrigger("Attack");
    }
}