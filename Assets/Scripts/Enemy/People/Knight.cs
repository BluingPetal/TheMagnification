using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : HumanoidEnemies
{
    [SerializeField]
    private KnightData data;

    private float runSpeed;

    // Properties -> ���ݽ� ���
    public Transform Target { get { return target; } }
    public float AttackPower { get { return attackPower; } }

    private void Awake()
    {
        name = data.name;
        description = data.description;
        icon = data.icon;
        prefab = data.prefab;

        hp = data.hp;
        speed = data.speed;
        attackRange = data.attackRange;
        attackRoutine = data.attackRoutine;
        attackPower = data.attackPower;
        runSpeed = data.runSpeed;
    }

    protected override void Move()
    {
        if (state == HumanoidEnemyState.Walk)
            base.Move();
        // ���� ���� ������Ʈ�� ���� ������ ��� �������� run speed�� �޷���
        else if (state == HumanoidEnemyState.WalkToTarget)
            MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            // target������ �̵�
            Vector3 posDiffWithTarget = target.position - this.transform.position;
            Vector3 dirToTargetWithoutY = new Vector3(posDiffWithTarget.x, 0, posDiffWithTarget.z).normalized;
            transform.Translate(dirToTargetWithoutY * runSpeed * Time.deltaTime, Space.World);
        }
    }

    protected override void FindTarget()
    {
        base.FindTarget();
        if (target != null)
        {
            // ���� ���� ������Ʈ�� �ڽź��� ���� �ִ� ���� ������ �� ����
            if ((target.position.y - transform.position.y) > (GetComponent<CapsuleCollider>().height + 0.5f))
            {
                target = null;
            }
        }
    }

    protected override void StateUpdate()
    {
        switch (state)
        {
            case HumanoidEnemyState.Walk:
                // Ÿ���� ������ walkToTarget state�� �̵�
                if (target != null)
                {
                    state = HumanoidEnemyState.WalkToTarget;
                    animator.SetTrigger("Attackable");
                    break;
                }
                break;

            case HumanoidEnemyState.WalkToTarget:
                if (target == null)
                {
                    // target������ ���ٰ� target�� ������ ��� ó��
                    state = HumanoidEnemyState.Walk;
                    animator.SetTrigger("Walk");
                    break;
                }
                Vector3 posDiffWithTarget1 = target.position - this.transform.position;
                float sqrdistanceToTarget1 = new Vector3(posDiffWithTarget1.x, 0, posDiffWithTarget1.z).sqrMagnitude;
                if (sqrdistanceToTarget1 < nearAttackTargetDistance)
                {
                    state = HumanoidEnemyState.Attack;
                    isMove = false;
                    Attack();
                    attackCoroutine = StartCoroutine(AttackDelay());
                    break;
                }
                break;

            case HumanoidEnemyState.Attack:
                // Ÿ���� ������ walk state�� �̵�
                if (target == null)
                {
                    state = HumanoidEnemyState.Walk;
                    animator.SetTrigger("Walk");
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    transform.LookAt(nextPos);
                    break;
                }
                Vector3 posDiffWithTarget2 = target.position - this.transform.position;
                float sqrdistanceToTarget2 = new Vector3(posDiffWithTarget2.x, 0, posDiffWithTarget2.z).sqrMagnitude;
                if (sqrdistanceToTarget2 >= nearAttackTargetDistance)
                {
                    state = HumanoidEnemyState.WalkToTarget;
                    isMove = true;
                    StopCoroutine(attackCoroutine);
                    animator.SetTrigger("ChangeTarget");
                    break;
                }
                break;
        }
    }

    protected override void Attack()
    {
        // Attack �ִϸ��̼ǿ��� Animation Event�� ���� target���� �������� ������ ����
        if (target != null)
        {
            Debug.Log("attack");
            animator.SetTrigger("Attack");
        }
    }

    public void GiveDamage() // Animation Event���� ȣ��� �Լ�
    {
        if (target != null)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            damageable?.TakeDamage(attackPower);
        }
    }
}
