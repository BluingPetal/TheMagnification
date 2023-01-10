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
    // scriptableObject에 공통적으로 있는 정보들
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

    // class 변수
    [HideInInspector]
    public int startWayNum = 1;        // 경로 종류
    private int nextIndex;          // 향하고 있는 인덱스
    protected Vector3 nextPos;      // 향하고 있는 인덱스의 위치
    private string curIndexName;    // enter된 오브젝트의 이름이 중복되면 index++되는 것을 방지하기 위함
    private string prevIndexName;
    [SerializeField]
    private float rotationSpeed;

    // child에서 쓰이는 변수
    protected Transform target;
    protected bool isMove;

    public UnityEvent<float, float> OnHpChanged;

    // Properties
    // public Vector3 CurPos { get { return this.transform.position; } }


    public float HP { get { return hp; } protected set { hp = value; OnHpChanged?.Invoke(maxHp, hp); } }

    virtual protected void Start()
    {
        // TODO : 정해진 위치에서 스폰되도록 구현
        //startWayNum = 1;
        //Debug.Log(string.Format("{0}번째 길을 선택", startWayNum));
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

        // layer가 WayPoints인 경우만 충돌
        if (other.gameObject.layer != LayerMask.NameToLayer("WayPoints"))
            return;
        // wayPoints의 부모와 이름이 같은 경우만 충돌
        if (other.gameObject.transform.parent.name != WayManager.Instance.WalkingWayName[startWayNum])
            return;
        // 중복해서 enter되었을 경우 방지
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
        // TODO : 적을 막지 못한 경우 처리
        Destroy(gameObject);
    }

    virtual protected void Attack() { }

    virtual public void TakeDamage(float damage) { }
}   
