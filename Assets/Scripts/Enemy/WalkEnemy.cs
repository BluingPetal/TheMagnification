using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WalkEnemy : MonoBehaviour
{
    // scriptableObject에 공통적으로 있는 정보들
    protected new string name;
    protected string description;
    protected Sprite icon;
    protected GameObject prefab;

    protected float hp;
    protected float speed;
    protected float attackRange;
    protected float attackRoutine;
    protected float attackPower;

    // class 변수
    private int startWayNum;
    private int nextIndex;
    private Vector3 nextPos;

    // child에서 쓰이는 변수
    protected Transform target;
    protected bool isMove;

    virtual protected void Start()
    {
        // TODO : 정해진 위치에서 스폰되도록 구현
        //randNum = Random.Range(0, 2);
        startWayNum = 1;
        Debug.Log(string.Format("{0}번째 길을 선택", startWayNum));
        nextIndex = 0;
        nextPos = WayManager.Instance.WalkingWayPoints[startWayNum][nextIndex].position;
        isMove = true;
        target = null;
        //agent.destination = WayManager.Instance.WalkingWayPoints[randNum][currentIndex].position;
    }

    virtual protected void Update()
    {
        if (!isMove)
            Move();
    }

    protected void Move()
    {
        Vector3 moveDir = new Vector3(nextPos.x - transform.position.x, 0, nextPos.z - transform.position.z).normalized;
        Vector3 lookPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
        transform.LookAt(lookPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        // wayPoints의 부모와 이름이 같은 경우만 충돌
        if (other.gameObject.transform.parent.name != WayManager.Instance.WalkingWayName[startWayNum])
            return;
        // layer가 WayPoints인 경우만 충돌
        if (other.gameObject.layer != LayerMask.NameToLayer("WayPoints"))
            return;

        if (nextIndex >= WayManager.Instance.WalkingWayPoints[startWayNum].Count - 1)
            ArrivedEndPoint();
        else
        {
            SetNextPoint();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
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

    virtual protected void Attack()
    {
    }
}   
