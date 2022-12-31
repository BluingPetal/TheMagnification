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

    protected override void Update()
    {
        if (isMove)
            Move();
        else
            MoveToTarget();
    }
    protected override void StateUpdate()
    {
        switch (state)
        {
            case HumanoidEnemyState.Walk:
                // 타겟이 있으면 attack state로 이동
                // 근접 공격을 하는 오브젝트는 다르게 처리
                if (target != null && this.name == "PoliceWithPole")
                {
                    state = HumanoidEnemyState.WalkToTarget;
                    isMove = false;
                    animator.SetTrigger("Attackable");
                    break;
                }
                if (target != null)
                {
                    state = HumanoidEnemyState.Attack;
                    isMove = false;
                    break;
                }
                break;

            case HumanoidEnemyState.WalkToTarget:
                if (target == null)
                {
                    // target쪽으로 가다가 target이 없어진 경우 처리
                    state = HumanoidEnemyState.Walk;
                    isMove = true;
                    animator.SetTrigger("StateChanged");
                    break;
                }
                float sqrdistanceToTarget = (target.position - this.transform.position).sqrMagnitude;
                if (sqrdistanceToTarget <= 2f)
                {
                    state = HumanoidEnemyState.Attack;
                    isMove = false;
                    Attack();
                    attackCoroutine = StartCoroutine(AttackDelay());
                    break;
                }
                break;

            case HumanoidEnemyState.Attack:
                // 타겟이 없으면 walk state로 이동
                if (target == null)
                {
                    Debug.Log("nullTarget");
                    state = HumanoidEnemyState.Walk;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("StateChanged");
                    break;
                }
                break;
        }
    }

    private void MoveToTarget()
    {
        if (state == HumanoidEnemyState.WalkToTarget && target != null)
        {
            // target쪽으로 이동
            Vector3 posDiffWithTarget = target.position - this.transform.position;
            Vector3 dirToTargetWithoutY = new Vector3(posDiffWithTarget.x, 0, posDiffWithTarget.z).normalized;
            transform.Translate(dirToTargetWithoutY * speed * Time.deltaTime, Space.World);
        }
    }

    protected void GiveDamage()
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(attackPower);
    }
}
