using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed;
    [HideInInspector]
    public float attackPower;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }

    private void Update()
    {
        // ���� �ð����� ��� ������ ���ư����� ����
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        DetectObject();
    }

    private void DetectObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            // �Ѿ�(�ڽ�) ����
            Destroy(gameObject);

            // ray�� ���� IDamageable Gameobject(target) ����
            IDamageable damageableTarget = hit.transform.gameObject.GetComponent<IDamageable>();
            damageableTarget?.TakeDamage(attackPower);
        }
    }
}
