using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class WalkEnemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    private int currentIndex = 0;
    private int randNum;

    protected new string name;
    protected string description;
    protected Sprite icon;
    protected GameObject prefab;

    protected float hp;
    protected float speed;
    protected float attackRange;
    protected float attackRoutine;
    protected float attackPower;

    virtual protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    virtual protected void Start()
    {
        // TODO : ���� ��ġ���� �����ǵ��� ����
        //randNum = Random.Range(0, 2);
        randNum = 1;
        agent.speed = speed;
        Debug.Log(string.Format("{0}��° ���� ����", randNum));
        //agent.destination = WayManager.Instance.WalkingWayPoints[randNum][currentIndex].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // wayPoints�� �θ�� �̸��� ���� ��츸 �浹
        if (other.gameObject.transform.parent.name != WayManager.Instance.WalkingWayName[randNum])
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("WayPoints"))
        {
            if (currentIndex >= WayManager.Instance.WalkingWayPoints[randNum].Count - 1)
                ArrivedEndPoint();
            else
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
        currentIndex++;
        agent.destination = WayManager.Instance.WalkingWayPoints[randNum][currentIndex].position;
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
