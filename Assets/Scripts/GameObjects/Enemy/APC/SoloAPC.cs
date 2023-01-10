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

        // 시작 weapon : 미사일
        curWeapon = Weapon.Missile;
        changedWeapon = true;
        animator.SetInteger("CurWeapon", (int)curWeapon);
        animator.SetBool("ChangedWeapon", changedWeapon);
    }

    protected override void LookAtTarget()
    {
        if (target != null)
        {
            // target을 바라보도록 구현
            if (curWeapon == Weapon.Bullet)
                topTransform.LookAt(target);
            else
                missileTransform.LookAt(target);

            // 타겟을 향한 ray 그리기
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
    }

    protected override void StateUpdate()
    {
        switch (state)
        {
            case CarState.Normal:
                // 타겟이 있으면 attack state로 이동
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
                        animator.SetInteger("CurWeapon", (int)curWeapon); // -> 애니메이터 바꿔주는 트리거 (총알에서 미사일로)
                        curWeapon = Weapon.Missile;
                        changedWeapon = true;
                        break;
                    }
                }
                else
                {
                    if (!changedWeapon)
                    {
                        animator.SetInteger("CurWeapon", (int)Weapon.Missile); // -> 애니메이터 바꿔주는 트리거 (미사일에서 총알로)
                        curWeapon = Weapon.Bullet;
                        changedWeapon = true;
                        break;
                    }
                }
                break;

            case CarState.Attack:
                // 타겟이 없으면 normal state로 이동
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
        // 총알 생성은 shooter가 진행
        // 타겟을 바로 공격하면 bullet과 싱크가 맞지 않으므로, bullet script에서 처리
        //animator.SetInteger("CurWeapon", (int)curWeapon);
        // 현재 weapon에 따라 발사되는 Shooter가 다름
        if (curWeapon == Weapon.Bullet)
            shooter.Shoot(target);
        else
            missileShooter.Shoot(target);

        animator.SetTrigger("Attack");
    }
}