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
            // ����ȭ�� ���� �ڷ�ƾ�� ����� 0.1�ʸ��� Ž���ϵ��� ����
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
            // 1. friend layer�� ���
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Friend")) // ���� layer�� �ƴ� �� �߰��ؾ� ��
                continue;

            // 2. �տ� ��ֹ��� ���� ���
            Vector3 posDiffWithTarget = colliders[i].gameObject.transform.position - this.transform.position;
            Vector3 dirToTargetWithoutY = new Vector3(posDiffWithTarget.x, 0, posDiffWithTarget.z).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTargetWithoutY, out hit, attackRange))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Friend"))
                    continue;
            }

            // 3. ��� ���� ���� -> IDamageable ������Ʈ�� �ִ� ��츸 Ÿ�� ���� ����
            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                // Ÿ���� ó�� ã���� ���
                if (target == null)
                {
                    target = colliders[i].transform;
                    float sqrDistanceToFirstTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
                    minSqrDistance = sqrDistanceToFirstTarget;
                }
                else // ���� Ÿ���� �־��� ��� �Ÿ� ���ؼ� �Ÿ��� �� ª�� ������Ʈ�� Ÿ������ ����
                {
                    float sqrDistanceToTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
                    if (sqrDistanceToTarget < minSqrDistance)
                    {
                        target = colliders[i].transform;
                        minSqrDistance = sqrDistanceToTarget;
                    }
                }

                // target�� �������� ���� ��쿡�� ����
                if (!target.IsDestroyed())
                {
                    // target���Ը� lookAt
                    // target�� ��ǥ�� ���� rotation�Ǵ� ���� ���� ���� �ڽ��� y�� ����
                    transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                    // Ÿ���� ���� ray �׸���
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
                // Ÿ���� ������ attack state�� �̵�
                if (target != null)
                {
                    state = HumanoidEnemyState.Attack;
                    isMove = false;
                    break;
                }
                break;

            case HumanoidEnemyState.Attack:
                // Ÿ���� ������ walk state�� �̵�
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
            // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��
            //IDamageable damageableTarget = target.gameObject.GetComponent<IDamageable>();
            //damageableTarget.TakeDamage(attackPower);

            // �Ѿ� ������ shooter�� ��
            // ���� ������ ��� �ִϸ��̼� �̺�Ʈ�� ����� �ִϸ��̼ǿ� ���� ����
            animator.SetTrigger("Attack");
        }
    }
}
