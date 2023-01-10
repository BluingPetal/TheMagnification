using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class WalkEnemy : MonoBehaviour, IDamageable
{
    // scriptableObject�� ���������� �ִ� ������
    protected new string name;
    protected string description;
    protected Sprite icon;
    protected GameObject prefab;
    protected int cost;

    protected float maxHp;
    protected float hp;
    protected float maxSpeed;
    protected float speed;
    protected float attackRange;
    protected float attackRoutine;
    protected float attackPower;

    // class ����
    [HideInInspector]
    public int startWayNum = 1;        // ��� ����
    private int nextIndex;          // ���ϰ� �ִ� �ε���
    protected Vector3 nextPos;      // ���ϰ� �ִ� �ε����� ��ġ
    private string curIndexName;    // enter�� ������Ʈ�� �̸��� �ߺ��Ǹ� index++�Ǵ� ���� �����ϱ� ����
    private string prevIndexName;
    [SerializeField]
    private float rotationSpeed;

    // child���� ���̴� ����
    protected Transform target;
    protected bool isMove;

    public UnityEvent<float, float> OnHpChanged;

    // Properties
    // public Vector3 CurPos { get { return this.transform.position; } }


    public float HP { get { return hp; } protected set { hp = value; OnHpChanged?.Invoke(maxHp, hp); } }

    virtual protected void Start()
    {
        // TODO : ������ ��ġ���� �����ǵ��� ����
        //startWayNum = 1;
        //Debug.Log(string.Format("{0}��° ���� ����", startWayNum));
        nextIndex = 0;
        nextPos = WayManager.Instance.WalkingWayPoints[startWayNum][nextIndex].position;
        curIndexName = WayManager.Instance.WalkingWayPoints[startWayNum][nextIndex].name;
        prevIndexName = "";
        isMove = true;
        target = null;
    }

    virtual protected void Update()
    {
        if (isMove)
            Move();
    }

    virtual protected void Move()
    {
        Vector3 moveDir = new Vector3(nextPos.x - transform.position.x, 0, nextPos.z - transform.position.z).normalized;
        //Vector3 lookPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
        transform.localRotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotationSpeed);
        //transform.LookAt(lookPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        curIndexName = other.gameObject.name;

        // layer�� WayPoints�� ��츸 �浹
        if (other.gameObject.layer != LayerMask.NameToLayer("WayPoints"))
            return;
        // wayPoints�� �θ�� �̸��� ���� ��츸 �浹
        if (other.gameObject.transform.parent.name != WayManager.Instance.WalkingWayName[startWayNum])
            return;
        // �ߺ��ؼ� enter�Ǿ��� ��� ����
        if (curIndexName == prevIndexName)
            return;

        if (nextIndex >= WayManager.Instance.WalkingWayPoints[startWayNum].Count - 1)
            ArrivedEndPoint();
        else
            SetNextPoint();

        prevIndexName = curIndexName;

        Debug.Log(other.transform.gameObject.name);
        Debug.Log(nextIndex);
    }

    virtual protected void OnDrawGizmosSelected()
    {
        if(target == null)
            Gizmos.color = Color.cyan;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void SetNextPoint()
    {
        nextIndex++;
        nextPos = WayManager.Instance.WalkingWayPoints[startWayNum][nextIndex].position;
    }
    
    virtual protected void ArrivedEndPoint()
    {
        // TODO : ���� ���� ���� ��� ó��
        Destroy(gameObject);
    }

    virtual protected void Attack() { }

    virtual public void TakeDamage(float damage) { }
}   
