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
            // ����ȭ�� ���� �ڷ�ƾ�� ����� 0.1�ʸ��� Ž���ϵ��� ����
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
            // 1. friend layer�� ��� ( // TODO : ���� layer�� �߰� �ʿ�)
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Friend"))
                continue;
            // 2. Target�� ���� ��� ( // TODO : state �����ؼ� ó�� �ʿ�)
            else if (colliders[i].gameObject.IsDestroyed())
                continue;

            // 3. �տ� ��ֹ��� ���� ���
            Vector3 posDiffWithTarget = (colliders[i].gameObject.transform.position - this.transform.position);
            Vector3 dirToTarget = posDiffWithTarget.normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit, attackRange))
            {
                // �Ѿ� ���̾�� ��ֹ��̶�� �������� ����
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                { 
                    // Do Nothing
                }
                else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Friend"))
                    continue;
                else if (hit.collider.gameObject.IsDestroyed())
                    continue;
            }

            // 4. ��� ���� ���� -> IDamageable ������Ʈ�� �ִ� ��츸 Ÿ�� ���� ����
            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                // Ÿ���� ó�� ã���� ���
                if (target == null)
                {
                    target = colliders[i].gameObject.transform;
                    float sqrDistanceToFirstTarget = posDiffWithTarget.sqrMagnitude;
                    minSqrDistance = sqrDistanceToFirstTarget;
                }
                else // ���� Ÿ���� �־��� ��� �Ÿ� ���ؼ� �Ÿ��� �� ª�� ������Ʈ�� Ÿ������ ����
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
            // target�� �ٶ󺸵��� ����
            topTransform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            // �ڽ��� y��ġ���� target�� y��ġ�� �� ���� ��� ��ü�� ���� �ٶ󺸵��� ����
            if (target.position.y > transform.position.y)
                topTransform.LookAt(target);

            // Ÿ���� ���� ray �׸���
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
    }
    
    private void StateUpdate()
    {
        switch (state)
        {
            case CarState.Normal:
                // Ÿ���� ������ attack state�� �̵�
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
                // Ÿ���� ������ normal state�� �̵�
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
            // TODO : Attack �Լ��� child���� ������
            Attack();
        }
    }
}
