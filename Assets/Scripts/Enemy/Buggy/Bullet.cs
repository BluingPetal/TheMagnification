using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float lifeTime;
    [HideInInspector]
    public float attackPower;
    [HideInInspector]
    public Transform target;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        FollowTarget();
        DetectObject();
    }

    public void FollowTarget()
    {
        if (target == null)
        {
            // ���� �ӵ��� ������ ���ư����� ����
            transform.Translate(transform.forward * bulletSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // ���� �ӵ��� Ÿ�� ��ġ�� ���󰡵��� ����
            Vector3 dirToTarget = (target.transform.position - this.transform.position).normalized;
            transform.Translate(dirToTarget * bulletSpeed * Time.deltaTime, Space.World);
            transform.LookAt(target.transform.position);
        }
    }

    private void DetectObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            // �Ѿ�(�ڽ�) ����
            Destroy(gameObject);

            // ray�� ���� IDamageable Gameobject(target) ����
            // Friend layer�� ��쿡�� IDamageable ó��
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Friend"))
            {
                IDamageable damageableTarget = hit.transform.gameObject.GetComponent<IDamageable>();
                damageableTarget?.TakeDamage(attackPower);
            }
        }
    }
}
