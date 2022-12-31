using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class WalkEnemy : MonoBehaviour
{
    // scriptableObject�� ���������� �ִ� ������
    protected new string name;
    protected string description;
    protected Sprite icon;
    protected GameObject prefab;

    protected float hp;
    protected float speed;
    protected float attackRange;
    protected float attackRoutine;
    protected float attackPower;

    // class ����
    private int startWayNum;
    private int nextIndex;
    private Vector3 nextPos;

    // child���� ���̴� ����
    protected Transform target;
    protected bool isMove;

    virtual protected void Start()
    {
        // TODO : ������ ��ġ���� �����ǵ��� ����
        //randNum = Random.Range(0, 2);
        startWayNum = 1;
        Debug.Log(string.Format("{0}��° ���� ����", startWayNum));
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
        // wayPoints�� �θ�� �̸��� ���� ��츸 �浹
        if (other.gameObject.transform.parent.name != WayManager.Instance.WalkingWayName[startWayNum])
            return;
        // layer�� WayPoints�� ��츸 �浹
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
        // TODO : ���� ���� ���� ��� ó��
        Destroy(gameObject);
    }

    virtual protected void Attack()
    {
    }
}   
