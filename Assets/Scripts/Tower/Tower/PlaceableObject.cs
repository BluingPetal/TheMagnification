using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    // data�� �ִ� ����
    protected new string name;
    protected Sprite icon;
    protected float attackRange;
    protected float attackRoutine;
    protected float attackPower;
    protected int cost;

    // build�Ǳ� ���� ����
    public bool isInstalled = false;
    private bool isAttack = false;

    // attack�ϱ� ���� ����
    protected Transform topTransform;   // target�� ���� ���� ���� transform

    private Coroutine searchTargetCoroutine;
    protected Coroutine attackCoroutine;
    private WaitForSeconds searchSeconds;
    protected WaitForSeconds attackSeconds;

    private float updateIntervalTime;   // Ÿ���� search�� ����
    private float minSqrDistance;       // �� ����� Ÿ���� �����ϱ� ���� �ּ� �Ÿ��� ����

    protected Transform target;

    // Properties - shop item���� ����ϱ� ���� -> ���߿� ���ֱ� �����͸� ������ ����
    public string Name { get { return name; } }
    public Sprite Icon { get { return icon; } }
    public int Cost { get { return cost; } }
    public float AttackPower { get { return attackPower; } }
    public float AttackRoutine { get { return attackRoutine; } }

    virtual protected void Start()
    {
        SetData();          // �ڽĿ��� ���� �ڽ��� data�� �޾ƿ� ��������
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
            // 1. Enemy layer�� ���
            if (colliders[i].gameObject.layer != LayerMask.NameToLayer("Enemy"))
                continue;
            // 2. Target�� ���� ��� -> layer�� DeadObject�� ��ȯ��
            else if (colliders[i].gameObject.layer == LayerMask.NameToLayer("DeadObject"))
                continue;

            // 3. �տ� ��ֹ��� ���� ��� -> backGroundLayer�� �������� ��� Ÿ������ �ν����� ����
            Vector3 posDiffWithTarget = (colliders[i].gameObject.transform.position - this.transform.position);
            Vector3 dirToTarget = posDiffWithTarget.normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dirToTarget, out hit, attackRange, LayerMask.NameToLayer("BackGround")))
            {
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

        if(target != null && !isAttack) // ���� ���� ��� bool ������ attack���� �ν�
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
            topTransform.LookAt(target);

            // Ÿ���� ���� ray �׸���
            Vector3 posDiffWithTarget = target.gameObject.transform.position - this.transform.position;
            Debug.DrawRay(transform.position, posDiffWithTarget, Color.yellow);
        }
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
    public void Sell() // -> ���߿� shop���� ���� ����
    {
        PlayerStatManager.Instance.MoneyChange((int)(cost * 0.5));
    }

    virtual protected void Attack() { }

    virtual protected void SetData() { }

    virtual protected void OnDrawGizmos()
    {
        if (target == null)
            Gizmos.color = Color.cyan;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
