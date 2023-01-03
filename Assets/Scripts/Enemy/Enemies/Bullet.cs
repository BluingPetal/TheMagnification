using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;
    [HideInInspector]
    public float attackPower;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public float bulletScale;

    private CapsuleCollider colldier;

    private void Awake()
    {
        colldier = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        this.gameObject.transform.localScale = this.gameObject.transform.lossyScale * bulletScale;
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
            // 일정 속도로 앞으로 나아가도록 구현
            transform.Translate(transform.forward * bulletSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // 일정 속도로 타겟 위치를 따라가도록 구현
            Vector3 dirToTarget = (target.transform.position - this.transform.position).normalized;
            transform.Translate(dirToTarget * bulletSpeed * Time.deltaTime, Space.World);
            transform.LookAt(target.transform.position);
        }
    }

    private void DetectObject()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, colldier.radius, transform.forward, out hit, colldier.height * transform.localScale.x))
        {
            // ray에 맞은 IDamageable Gameobject(target) 반응
            // Friend layer일 경우에만 IDamageable 처리
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Friend"))
            {
                IDamageable damageableTarget = hit.transform.gameObject.GetComponent<IDamageable>();
                damageableTarget?.TakeDamage(attackPower);
            }

            // 총알(자신) 반응
            Destroy(gameObject);
        }
        Debug.DrawRay(transform.position, transform.forward * (colldier.height * transform.localScale.x), Color.magenta);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Friend"))
        {
            IDamageable damageableTarget = other.gameObject.GetComponent<IDamageable>();
            damageableTarget?.TakeDamage(attackPower);
        }

        // 총알(자신) 반응
        Destroy(gameObject);
    }
}
