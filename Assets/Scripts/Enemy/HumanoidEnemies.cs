using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum HumanoidEnemyState { Walk, Attack }
public class HumanoidEnemies : WalkEnemy
{
    protected Animator animator;

    private HumanoidEnemyState state;
    protected Transform target;
    private Coroutine attackCoroutine;

    private float minSqrDistance;
    private bool isAttack;
    private bool isFound;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        state = HumanoidEnemyState.Walk;
        target = null;
        isAttack = false;
        isFound = false;
    }


    private void Update()
    {
        FindTarget();
        StateUpdate();
    }

    private void StateUpdate()
    {
        switch (state)
        {
            case HumanoidEnemyState.Walk:
                agent.isStopped = false;
                // 타겟이 있으면 attack state로 이동
                if (target != null)
                    state = HumanoidEnemyState.Attack;
                break;

            case HumanoidEnemyState.Attack:
                agent.isStopped = true;
                // 타겟이 없으면 attack state로 이동
                if (target == null)
                {
                    Debug.Log("nullTarget");
                    state = HumanoidEnemyState.Walk;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("StateChanged");
                    isAttack = false;
                }
                if (!isAttack)
                {
                    Attack();
                    attackCoroutine = StartCoroutine(AttackDelay());
                    isAttack = true;
                }
                break;
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
            Vector3 directionToTarget = (colliders[i].gameObject.transform.position - this.transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, attackRange))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Friend"))
                    continue;
            }

            // 3. 모든 조건 충족 -> IDamageable 오브젝트가 있는 경우만 타겟 설정
            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                // 타겟을 처음 찾았을 경우
                if (!isFound)
                {
                    target = colliders[i].transform;
                    float sqrDistanceToFirstTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
                    minSqrDistance = sqrDistanceToFirstTarget;
                    isFound = true;
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
                    transform.LookAt(target.position);
                    // 타겟을 향한 ray 그리기
                    Vector3 directionToFinalTarget = (target.gameObject.transform.position - this.transform.position).normalized;
                    Debug.DrawRay(transform.position, directionToFinalTarget * attackRange);
                    Debug.Log(target.ToString());
                }
                else
                {
                    isFound = false;
                    target = null;
                }
            }
            else
            {
                isFound = false;
                target = null;
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackRoutine);
            Debug.Log("attackDelay");
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
            animator.SetTrigger("Attack");
        }
    }
}
