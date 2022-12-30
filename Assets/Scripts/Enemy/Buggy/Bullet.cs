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
        // 일정 시간으로 계속 앞으로 날아가도록 구현
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        DetectObject();
    }

    private void DetectObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            // 총알(자신) 반응
            Destroy(gameObject);

            // ray에 맞은 IDamageable Gameobject(target) 반응
            IDamageable damageableTarget = hit.transform.gameObject.GetComponent<IDamageable>();
            damageableTarget?.TakeDamage(attackPower);
        }
    }
}
