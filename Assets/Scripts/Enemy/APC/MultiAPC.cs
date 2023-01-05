using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 미사일과 총알을 동시에 생성 가능한 APC
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

        // bullet 생성하는 shooter가 활성화 되었는가 여부
        bulletShooterActived = false;
    }

    protected override void LookAtTarget()
    {
        if (target != null)
        {
            // target을 바라보도록 구현
            if (bulletShooterActived)
                topTransform.LookAt(new Vector3(target.position.x, topTransform.position.y, target.position.z));
                
            missileTransform.LookAt(new Vector3(target.position.x, missileTransform.position.y, target.position.z));

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
                    // 타겟과의 거리를 계산해 bullet shooter의 활성화 여부 결정
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
                // 타겟이 없으면 normal state로 이동
                if (target == null)
                {
                    Debug.Log("nullTarget");
                    state = CarState.Normal;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("Normal");
                    break;
                }

                // 타겟과의 거리를 계산해 bullet shooter의 활성화 여부 결정
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
        // 총알 생성은 shooter가 진행
        // 타겟을 바로 공격하면 bullet과 싱크가 맞지 않으므로, bullet script에서 처리

        // 현재 bullet shooter 활성화 여부에 따라 발사되는 Shooter의 개수가 다름
        if (bulletShooterActived)
        {
            shooter.Shoot(target);
            shooter1.Shoot(target);
        }

        missileShooter.Shoot(target);

        animator.SetTrigger("Attack");
    }
}
