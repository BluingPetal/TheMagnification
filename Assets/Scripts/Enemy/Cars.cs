//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;

//enum CarState { Normal, Attack }

//public class Cars : WalkEnemy
//{
//    private CarState state;
//    private Transform target;
//    protected GameObject bulletPrefab;
//    private Coroutine attackCoroutine;

//    private float minSqrDistance;
//    private bool isAttack;
//    private bool isFound;

//    [Header("Settings")]
//    [SerializeField]
//    protected Animator animator;
//    [SerializeField]
//    protected Shooter shooter;
//    [SerializeField]
//    protected Transform topTransform;
//    [SerializeField]
//    protected Transform bottomTransform;

//    protected override void Start()
//    {
//        base.Start();
//        state = CarState.Normal;
//        target = null;
//        isAttack = false;
//        isFound = false;
//    }


//    private void Update()
//    {
//        FindTarget();
//        StateUpdate();
//    }

//    private void StateUpdate()
//    {
//        switch (state)
//        {
//            case CarState.Normal:
//                // Ÿ���� ������ attack state�� �̵�
//                if (target != null)
//                {
//                    state = CarState.Attack;
//                    agent.isStopped = true;
//                    break;
//                }
//                break;

//            case CarState.Attack:
//                // Ÿ���� ������ attack state�� �̵�
//                if (target == null)
//                {
//                    Debug.Log("nullTarget");
//                    state = CarState.Normal;
//                    StopCoroutine(attackCoroutine);
//                    agent.isStopped = false;
//                    animator.SetTrigger("StateChanged");
//                    isAttack = false;
//                    break;
//                }
//                if (!isAttack)
//                {
//                    Attack();
//                    attackCoroutine = StartCoroutine(AttackDelay());
//                    isAttack = true;
//                }
//                break;
//        }
//    }

//    private void FindTarget()
//    {
//        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange);

//        for (int i = 0; i < colliders.Length; i++)
//        {
//            // 1. friend layer�� ���
//            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Friend")) // ���� layer�� �ƴ� �� �߰��ؾ� ��
//                continue;

//            // 2. �տ� ��ֹ��� ���� ���
//            Vector3 directionToTarget = (colliders[i].gameObject.transform.position - this.transform.position).normalized;
//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, directionToTarget, out hit, attackRange))
//            {
//                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Friend"))
//                    continue;
//            }

//            // 3. ��� ���� ���� -> IDamageable ������Ʈ�� �ִ� ��츸 Ÿ�� ����
//            IDamageable damageableObj = colliders[i].GetComponent<IDamageable>();
//            if (damageableObj != null)
//            {
//                // Ÿ���� ó�� ã���� ���
//                if (!isFound)
//                {
//                    target = colliders[i].transform;
//                    float sqrDistanceToFirstTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
//                    minSqrDistance = sqrDistanceToFirstTarget;
//                    isFound = true;
//                }
//                else // ���� Ÿ���� �־��� ��� �Ÿ� ���ؼ� �Ÿ��� �� ª�� ������Ʈ�� Ÿ������ ����
//                {
//                    float sqrDistanceToTarget = (colliders[i].gameObject.transform.position - this.transform.position).sqrMagnitude;
//                    if (sqrDistanceToTarget < minSqrDistance)
//                    {
//                        target = colliders[i].transform;
//                        minSqrDistance = sqrDistanceToTarget;
//                    }
//                }

//                // target�� �������� ���� ��쿡�� ����
//                if (!target.IsDestroyed())
//                {
//                    // target���Ը� lookAt
//                    // target�� ��ǥ�� ���� rotation�Ǵ� ���� ���� ���� �ڽ��� y�� ����
//                    //topTransform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
//                    topTransform.LookAt(target.position);
//                    // Ÿ���� ���� ray �׸���
//                    Vector3 directionToFinalTarget = (target.gameObject.transform.position - this.transform.position).normalized;
//                    Debug.DrawRay(transform.position, directionToFinalTarget * attackRange);
//                    Debug.Log(target.ToString());
//                }
//                else
//                {
//                    isFound = false;
//                    target = null;
//                }
//            }
//            else
//            {
//                isFound = false;
//                target = null;
//            }
//        }
//    }

//    private IEnumerator AttackDelay()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(attackRoutine);
//            Debug.Log("attackDelay");
//            Attack();
//        }
//    }

//    protected override void Attack()
//    {
//        if(target != null)
//        {
//            // Ÿ���� �ٷ� �����ϸ� bullet�� ��ũ�� ���� �����Ƿ�, bullet script���� ó��
//            //IDamageable damageableTarget = target.gameObject.GetComponent<IDamageable>();
//            //damageableTarget.TakeDamage(attackPower);

//            // �Ѿ� ������ shooter�� ��
//            shooter.Shoot(bulletPrefab, attackPower);
//            animator.SetTrigger("Attack");
//        }
//    }
//}
