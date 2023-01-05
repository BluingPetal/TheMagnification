using Core.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooter : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public string kind = "";

    private GameObject bulletPrefab;
    private float bulletSpeed;
    private float attackPower;
    private float bulletScale;

    private void Start()
    {
        Debug.Log(kind);
        // 부모찾기 TODO : 오브젝트 추가
        // 부모의 data를 받아와 대입
        if (owner.TryGetComponent(out Buggy buggy))
        {
            Debug.Log(string.Format("{0} : buggy2", owner.name));
            bulletPrefab = buggy.data.bulletPrefab;
            bulletSpeed = buggy.data.bulletSpeed;
            attackPower = buggy.data.attackPower;
            bulletScale = buggy.data.bulletScale;
            return;
        }
        if (owner.TryGetComponent(out SoloAPC apc))
        {
            Debug.Log(string.Format("{0} : apc", owner.name));
            if (kind == "Missile")
            {
                Debug.Log("missile");
                bulletPrefab = apc.data.missilePrefab;
                bulletSpeed = apc.data.missileSpeed;
                attackPower = apc.data.missilePower;
                bulletScale = apc.data.missileScale;
            }
            else
            {
                Debug.Log("bullet");
                bulletPrefab = apc.data.bulletPrefab;
                bulletSpeed = apc.data.bulletSpeed;
                attackPower = apc.data.bulletPower;
                bulletScale = apc.data.bulletScale;
            }
            return;
        }
        if (owner.TryGetComponent(out MultiAPC multiAPC))
        {
            Debug.Log(string.Format("{0} : multiAPC", owner.name));
            if (kind == "Missile")
            {
                Debug.Log("missile");
                bulletPrefab = multiAPC.data.missilePrefab;
                bulletSpeed = multiAPC.data.missileSpeed;
                attackPower = multiAPC.data.missilePower;
                bulletScale = multiAPC.data.missileScale;
            }
            else
            {
                Debug.Log("bullet");
                bulletPrefab = multiAPC.data.bulletPrefab;
                bulletSpeed = multiAPC.data.bulletSpeed;
                attackPower = multiAPC.data.bulletPower;
                bulletScale = multiAPC.data.bulletScale;
            }
            return;
        }
        PoliceWithPistol policeWithPistol = owner.GetComponent<PoliceWithPistol>();
        if (policeWithPistol != null)
        {
            Debug.Log(string.Format("{0} : policeWithPistol", owner.name));
            bulletPrefab = policeWithPistol.data.bulletPrefab;
            bulletSpeed = policeWithPistol.data.bulletSpeed;
            attackPower = policeWithPistol.data.attackPower;
            bulletScale = policeWithPistol.data.bulletScale;
            return;
        }
        Soldier soldier = owner.GetComponent<Soldier>();
        if (soldier != null)
        {
            Debug.Log(string.Format("{0} : soldier",owner.name));
            bulletPrefab = soldier.data.bulletPrefab;
            bulletSpeed = soldier.data.bulletSpeed;
            attackPower = soldier.data.attackPower;
            bulletScale = soldier.data.bulletScale;
            return;
        }
    }

    public void Shoot(Transform target)
    {
            GameObject obj = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.target = target;
            bullet.attackPower = attackPower;
            bullet.bulletSpeed = bulletSpeed;
            bullet.bulletScale = bulletScale;
    }
}
