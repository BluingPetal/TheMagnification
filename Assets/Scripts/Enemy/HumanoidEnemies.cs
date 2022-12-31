using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum HumanoidEnemyState { Walk, WalkToTarget, Attack }

[RequireComponent(typeof(Animator))]
public class HumanoidEnemies : WalkEnemy
{
    protected Animator animator;

    protected HumanoidEnemyState state;
    private Coroutine searchTargetCoroutine;
    protected Coroutine attackCoroutine;

    private float updateIntervalTime;
    private float minSqrDistance;

    virtual protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        state = HumanoidEnemyState.Walk;
        searchTargetCoroutine = StartCoroutine(FindTargetDelay());
        updateIntervalTime = 0.1f;
    }

    private IEnumerator FindTargetDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateIntervalTime);
            // 최적화를 위해 코루틴을 사용해 0.1초마다 탐색하도록 구현
            FindTarget();
            StateUpdate();
            Debug.Log(state);
        }
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            // 1. friend layer일 경우
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Friend")) // 기지 layer도 아닐 때 추가해야 함
                continue;

            // 2. 앞에 장애물이 없을 경우
            Vector3 posDiffWithTarget = colliders[i].gameObject.transform.position - this.transform.position;
            Vector3 dirToTargetWithoutY = new Vector3(posDiffWithTarget.x, 0, posDiffWithTarget.z).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTargetWithoutY, out hit, attackRange))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Friend"))
                    continue;
            }

            // 3. 모든 조건 충족 -> IDamageable 오브젝트가 있는 경우만 타겟 설정 가능
            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                // 타겟을 처음 찾았을 경우
                if (target == null)
                {
                    target = colliders[i].transform;
                    float sqrDistanceToFirstTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
                    minSqrDistance = sqrDistanceToFirstTarget;
                }
                else // 이전 타겟이 있었던 경우 거리 비교해서 거리가 더 짧은 오브젝트를 타겟으로 변경
                {
                    float sqrDistanceToTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
                    if (sqrDistanceToTarget < minSqrDistance)
                    {
                        target = colliders[i].transform;
                        minSqrDistance = sqrDistanceToTarget;
                    }
                }

                // target이 없어지지 않은 경우에만 실행
                if (!target.IsDestroyed())
                {
                    // target에게만 lookAt
                    // target의 좌표에 따라 rotation되는 것을 막기 위해 자신의 y값 대입
                    transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                    // 타겟을 향한 ray 그리기
                    Debug.DrawRay(transform.position, dirToTargetWithoutY * attackRange);
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                target = null;
            }
        }
    }

    virtual protected void StateUpdate()
    {
        switch (state)
        {
            case HumanoidEnemyState.Walk:
                // 타겟이 있으면 attack state로 이동
                if (target != null)
                {
                    state = HumanoidEnemyState.Attack;
                    isMove = false;
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

    protected IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackRoutine);
            Attack();
        }
    }

    protected override void Attack()
    {
        if (target != null)
        {
            // 타겟을 바로 공격하면 bullet과 싱크가 맞지 않으므로, bullet script에서 처리
            //IDamageable damageableTarget = target.gameObject.GetComponent<IDamageable>();
            //damageableTarget.TakeDamage(attackPower);

            // 총알 생성은 shooter가 함
            // 근접 공격인 경우 애니메이션 이벤트를 사용해 애니메이션에 맞춰 공격
            animator.SetTrigger("Attack");
        }
    }
}
