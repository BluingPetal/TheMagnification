using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableObject : MonoBehaviour, IDamageable
{
    protected new string name;
    protected float maxHp;
    protected float curHp;
    protected float attackRange;
    protected float attackRoutine;
    [HideInInspector]
    public float attackPower;
    protected int cost;

    public bool isInstalled = false;
    private bool isAttack = false;

    protected Transform topTransform;

    private Coroutine searchTargetCoroutine;
    protected Coroutine attackCoroutine;
    WaitForSeconds searchSeconds;
    protected WaitForSeconds attackSeconds;

    private float updateIntervalTime;
    private float minSqrDistance;

    protected Transform target;

    virtual protected void Start()
    {
        Debug.Log("start");
        updateIntervalTime = 0.1f;
        searchSeconds = new WaitForSeconds(updateIntervalTime);
        attackSeconds = new WaitForSeconds(attackRoutine);
        if (isInstalled)
            searchTargetCoroutine = StartCoroutine(FindTargetDelay());
    }

    private void Update()
    {
        if (isInstalled)
            LookAtTarget();
    }

    private IEnumerator FindTargetDelay()
    {
        while (true)
        {
            yield return searchSeconds;
            // 최적화를 위해 코루틴을 사용해 0.1초마다 탐색하도록 구현
            FindTarget();
        }
    }

    private void FindTarget()
    {
        target = null; // 처음 target == null
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            // 1. enemy layer일 경우
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;
            // 2. Target이 죽은 경우 ( // TODO : state 참조해서 처리 필요)
            else if (colliders[i].gameObject.layer == LayerMask.NameToLayer("DeadObject"))
                continue;

            // 3. 앞에 장애물이 없을 경우
            Vector3 posDiffWithTarget = (colliders[i].gameObject.transform.position - this.transform.position);
            Vector3 dirToTarget = posDiffWithTarget.normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit, attackRange))
            {
                // 총알 레이어는 장애물이라고 생각하지 않음
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("FriendBullet"))
                {
                    // Do Nothing
                }
                else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                    continue;
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("DeadObject"))
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

        if(target != null && !isAttack)
        {
            Attack();
            attackCoroutine = StartCoroutine(AttackDelay());
            isAttack = true;
        }
        else if(target == null && isAttack)
        {
            StopCoroutine(attackCoroutine);
            isAttack = false;
        }
    }
    protected void LookAtTarget()
    {
        if (target != null)
        {
            // target을 바라보도록 구현
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            // 자신의 y위치보다 target의 y위치가 더 높을 경우 상체만 위를 바라보도록 구현
            // TODO : 공중 오브젝트 있을 때 주석 풀기 ( 다른 곳에서 쏠때의 파티클 시스템이 작동함 )
            //if (target.position.y > transform.position.y)
            //    topTransform.LookAt(target);

            // 타겟을 향한 ray 그리기
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
    }
    public void Sell()
    {
        PlayerStatManager.Instance.MoneyChange((int)(cost * 0.5));
    }
    virtual protected IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return attackSeconds;
            // TODO : Attack 함수는 child에서 재정의
            Attack();
        }
    }

    virtual protected void Attack() { }

    virtual protected void OnDrawGizmos()
    {
        if (target == null)
            Gizmos.color = Color.cyan;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    virtual public void TakeDamage(float damage) { }
}
