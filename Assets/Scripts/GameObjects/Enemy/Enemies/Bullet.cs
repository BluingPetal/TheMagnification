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
    public LayerMask ownerLayer;
    [HideInInspector]
    public float attackPower;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public float bulletSpeed;
    [HideInInspector]
    public float bulletScale;
    [HideInInspector]
    public ParticleSystem explodeParticle = null;

    private CapsuleCollider colldier;
    private bool isAlreadyDamaged;

    private void Awake()
    {
        colldier = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        this.gameObject.transform.localScale = this.gameObject.transform.lossyScale * bulletScale;
        Destroy(gameObject, lifeTime);
        isAlreadyDamaged = false;
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
            Vector3 targetScale = new Vector3(0, target.localScale.y * 0.5f, 0);
            // ���� �ӵ��� Ÿ�� ��ġ�� ���󰡵��� ����
            Vector3 dirToTarget = (target.transform.position + targetScale - this.transform.position).normalized;
            transform.Translate(dirToTarget * bulletSpeed * Time.deltaTime, Space.World);
            transform.LookAt(target.transform.position + targetScale);
        }
    }

    private void DetectObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, colldier.height * 1.5f))
        {
            // ray�� ���� IDamageable Gameobject(target) ����
            // layer�� ���� ������ �ٸ��� ó��
            if ((ownerLayer == LayerMask.NameToLayer("Enemy") && hit.collider.gameObject.layer == LayerMask.NameToLayer("Friend"))
                || (ownerLayer == LayerMask.NameToLayer("Friend") && hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")))
            {
                Debug.Log("raycast dead");
                IDamageable damageableTarget = hit.transform.gameObject.GetComponent<IDamageable>();
                damageableTarget?.TakeDamage(attackPower);

                // explode ȿ��
                if (explodeParticle != null)
                    Instantiate(explodeParticle, hit.point, Quaternion.LookRotation(hit.normal));
                // �Ѿ�(�ڽ�) ����
                Destroy(gameObject);

                isAlreadyDamaged = true;
            }

            Debug.Log(string.Format("owner layer : {0}, hit layer : {1}", LayerMask.LayerToName(ownerLayer), hit.collider.gameObject.layer.ToString()));
            Debug.Log(string.Format("hit name : {0}", hit.collider.gameObject.name));
        }
        Debug.DrawRay(transform.position, transform.forward * (colldier.height * 1.5f), Color.magenta);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // target�� ray�� �������� ���� ��츦 ������ trigger������ ����
    //    // ray�� �ߺ� ������� �ʵ��� isAlreadyDamaged (bool)���� �߰� // layer�� ���� ������ �ٸ��� ó��
    //    if (((ownerLayer == LayerMask.NameToLayer("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Friend"))
    //        || (ownerLayer == LayerMask.NameToLayer("Friend") && other.gameObject.layer == LayerMask.NameToLayer("Enemy")))
    //        && !isAlreadyDamaged)
    //    {
    //        Debug.Log("trigger enter dead");
    //        Debug.Log(string.Format("owner layer : {0}, hit layer : {1}", LayerMask.LayerToName(ownerLayer), hit.collider.gameObject.layer.ToString()));
    //        Debug.Log(string.Format("hit name : {0}", hit.collider.gameObject.name));
    //        IDamageable damageableTarget = other.gameObject.GetComponent<IDamageable>();
    //        damageableTarget?.TakeDamage(attackPower);
    //    }
    //
    //    // explode ȿ��
    //    if (explodeParticle != null)
    //        Instantiate(explodeParticle, transform.position, transform.rotation);
    //    // �Ѿ�(�ڽ�) ����
    //    Destroy(gameObject);
    //}
}
