using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public enum HumanoidEnemyState { Walk, WalkToTarget, NearAttack, Attack }

public class HumanoidEnemies : WalkEnemy
{
    // �ΰ��� ������Ʈ ����
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    private Transform topTransform; // ��ü ����
    [SerializeField]
    protected float nearAttackTargetDistance;

    protected HumanoidEnemyState state;
    private Coroutine searchTargetCoroutine;
    protected Coroutine attackCoroutine;
    WaitForSeconds searchSeconds;
    WaitForSeconds attackSeconds;

    private float updateIntervalTime;
    private float minSqrDistance;

    protected override void Start()
    {
        base.Start();
        state = HumanoidEnemyState.Walk;
        updateIntervalTime = 0.1f;
        searchSeconds = new WaitForSeconds(updateIntervalTime);
        attackSeconds = new WaitForSeconds(attackRoutine);
        searchTargetCoroutine = StartCoroutine(FindTargetDelay());
    }

    private IEnumerator FindTargetDelay()
    {
        while (true)
        {
            yield return searchSeconds;
            // ����ȭ�� ���� �ڷ�ƾ�� ����� 0.1�ʸ��� Ž���ϵ��� ����
            FindTarget();
            LookAtTarget();
            StateUpdate();
            //Debug.Log(state);
        }
    }

    protected virtual void FindTarget()
    {
        target = null; // ó�� target == null
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
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
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
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            // �ڽ��� y��ġ���� target�� y��ġ�� �� ���� ��� ��ü�� ���� �ٶ󺸵��� ����
            if (target.position.y > transform.position.y)
                topTransform.LookAt(target);

            // Ÿ���� ���� ray �׸���
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
        else
        {
            topTransform.rotation = transform.rotation;
        }
    }

    virtual protected void StateUpdate()
    {
        switch (state)
        {
            case HumanoidEnemyState.Walk:
                // Ÿ���� ������ attack state�� �̵�
                if (target != null)
                {
                    Vector3 posDiffWithTarget1 = target.position - this.transform.position;
                    float sqrdistanceToTarget1 = posDiffWithTarget1.sqrMagnitude;
                    if (sqrdistanceToTarget1 < nearAttackTargetDistance)
                        state = HumanoidEnemyState.NearAttack;
                    else
                        state = HumanoidEnemyState.Attack;

                    isMove = false;
                    Attack();
                    attackCoroutine = StartCoroutine(AttackDelay());
                    break;
                }
                break;

            case HumanoidEnemyState.NearAttack:
                // Ÿ���� ������ walk state�� �̵�
                if (target == null)
                {
                    state = HumanoidEnemyState.Walk;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("Walk");
                    transform.LookAt(nextPos);
                    break;
                }
                Vector3 posDiffWithTarget2 = target.position - this.transform.position;
                float sqrdistanceToTarget2 = posDiffWithTarget2.sqrMagnitude;
                if (sqrdistanceToTarget2 >= nearAttackTargetDistance)
                {
                    state = HumanoidEnemyState.Attack;
                    break;
                }
                break;

            case HumanoidEnemyState.Attack:
                // Ÿ���� ������ walk state�� �̵�
                if (target == null)
                {
                    state = HumanoidEnemyState.Walk;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("Walk");
                    transform.LookAt(nextPos);
                    break;
                }
                Vector3 posDiffWithTarget3 = target.position - this.transform.position;
                float sqrdistanceToTarget3 = posDiffWithTarget3.sqrMagnitude;
                if (sqrdistanceToTarget3 < nearAttackTargetDistance)
                {
                    state = HumanoidEnemyState.NearAttack;
                    break;
                }
                break;
        }
    }

    protected IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return attackSeconds;
            // TODO : Attack �Լ��� child���� ������
            Attack();
        }
    }

}
