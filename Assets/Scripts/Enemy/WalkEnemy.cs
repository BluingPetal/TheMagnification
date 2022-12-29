using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class WalkEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentIndex = 0;
    private int randNum;

    [SerializeField]
    protected float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // TODO : ���� ��ġ ����
        randNum = Random.Range(0, 1);
        agent.speed = speed;
        Debug.Log(string.Format("{0}��° ���� ����", randNum));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered");
        Debug.Log(currentIndex.ToString());
        if (other.gameObject.layer == LayerMask.NameToLayer("WayPoints"))
        {
            if (currentIndex >= WayManager.Instance.WalkingWayPoints[randNum].Count - 1)
                ArrivedEndPoint();
            else
                SetNextPoint();
        }
    }
    
    private void SetNextPoint()
    {
        currentIndex++;
        agent.destination = WayManager.Instance.WalkingWayPoints[randNum][currentIndex].position;
    }
    
    private void ArrivedEndPoint()
    {
        // TODO : ���� ���� ���� ��� ó��
        Destroy(gameObject);
    }
}   
