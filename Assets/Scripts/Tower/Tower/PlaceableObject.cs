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

    public int Cost { get { return cost; } }

    virtual protected void Start()
    {
        Debug.Log("start");
        SetData();
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
            // ����ȭ�� ���� �ڷ�ƾ�� ����� 0.1�ʸ��� Ž���ϵ��� ����
            FindTarget();
        }
    }

    private void FindTarget()
    {
        target = null; // ó�� target == null
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, attackRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            // 1. enemy layer�� ���
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;
            // 2. Target�� ���� ��� ( // TODO : state �����ؼ� ó�� �ʿ�)
            else if (colliders[i].gameObject.layer == LayerMask.NameToLayer("DeadObject"))
                continue;

            // 3. �տ� ��ֹ��� ���� ���
            Vector3 posDiffWithTarget = (colliders[i].gameObject.transform.position - this.transform.position);
            Vector3 dirToTarget = posDiffWithTarget.normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit, attackRange))
            {
                // �Ѿ� ���̾�� ��ֹ��̶�� �������� ����
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("FriendBullet"))
                {
                    // Do Nothing
                }
                else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                    continue;
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("DeadObject"))
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
            // target�� �ٶ󺸵��� ����
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            // �ڽ��� y��ġ���� target�� y��ġ�� �� ���� ��� ��ü�� ���� �ٶ󺸵��� ����
            // TODO : ���� ������Ʈ ���� �� �ּ� Ǯ�� ( �ٸ� ������ ���� ��ƼŬ �ý����� �۵��� )
            //if (target.position.y > transform.position.y)
            //    topTransform.LookAt(target);

            // Ÿ���� ���� ray �׸���
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
            // TODO : Attack �Լ��� child���� ������
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

    virtual protected void SetData() { }
}
