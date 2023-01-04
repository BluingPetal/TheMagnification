using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum CarState { Normal, Attack }

public class Cars : WalkEnemy
{
    // car setting
    [Header("Settings")]
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected Shooter shooter;
    [SerializeField]
    private Transform topTransform;

    private CarState state;
    protected GameObject bulletPrefab;

    private Coroutine searchTargetCoroutine;
    private Coroutine attackCoroutine;
    WaitForSeconds searchSeconds;
    WaitForSeconds attackSeconds;

    private float updateIntervalTime;
    private float minSqrDistance;

    protected override void Start()
    {
        base.Start();
        state = CarState.Normal;
        updateIntervalTime = 0.1f;
        searchSeconds = new WaitForSeconds(updateIntervalTime);
        attackSeconds = new WaitForSeconds(attackRoutine);
        searchTargetCoroutine = StartCoroutine(FindTargetDelay());
    }

    protected override void Update()
    {
        base.Update();
        LookAtTarget();
    }

    private IEnumerator FindTargetDelay()
    {
        while (true)
        {
            yield return searchSeconds;
            // 최적화를 위해 코루틴을 사용해 0.1초마다 탐색하도록 구현
            FindTarget();
            StateUpdate();
            //Debug.Log(state);
        }
    }
    private void FindTarget()
    {
        target = null;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            // 1. friend layer일 경우 ( // TODO : 기지 layer도 추가 필요)
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Friend"))
                continue;
            // 2. Target이 죽은 경우 ( // TODO : state 참조해서 처리 필요)
            else if (colliders[i].gameObject.IsDestroyed())
                continue;

            // 3. 앞에 장애물이 없을 경우
            Vector3 posDiffWithTarget = (colliders[i].gameObject.transform.position - this.transform.position);
            Vector3 dirToTarget = posDiffWithTarget.normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit, attackRange))
            {
                // 총알 레이어는 장애물이라고 생각하지 않음
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                { 
                    // Do Nothing
                }
                else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Friend"))
                    continue;
                else if (hit.collider.gameObject.IsDestroyed())
                    continue;
            }

            // 4. 모든 조건 충족 -> IDamageable 오브젝트가 있는 경우만 타겟 설정 가능
            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                // 타겟을 처음 찾았을 경우
                if (target == null)
                {
                    target = colliders[i].gameObject.transform;
                    float sqrDistanceToFirstTarget = posDiffWithTarget.sqrMagnitude;
                    minSqrDistance = sqrDistanceToFirstTarget;
                }
                else // 이전 타겟이 있었던 경우 거리 비교해서 거리가 더 짧은 오브젝트를 타겟으로 변경
                {
                    float sqrDistanceToTarget = posDiffWithTarget.sqrMagnitude;
                    if (sqrDistanceToTarget < minSqrDistance)
                    {
                        target = colliders[i].gameObject.transform;
                        minSqrDistance = sqrDistanceToTarget;
                    }
                }
            }
        }
    }
    protected void LookAtTarget()
    {
        if (target != null)
        {
            // target을 바라보도록 구현
            topTransform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            // 자신의 y위치보다 target의 y위치가 더 높을 경우 상체만 위를 바라보도록 구현
            if (target.position.y > transform.position.y)
                topTransform.LookAt(target);

            // 타겟을 향한 ray 그리기
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
    }
    
    private void StateUpdate()
    {
        switch (state)
        {
            case CarState.Normal:
                // 타겟이 있으면 attack state로 이동
                if (target != null)
                {
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
                    animator.SetTrigger("StateChanged");
                    break;
                }
                break;
        }
    }

    private IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackRoutine);
            // TODO : Attack 함수는 child에서 재정의
            Attack();
        }
    }
}
