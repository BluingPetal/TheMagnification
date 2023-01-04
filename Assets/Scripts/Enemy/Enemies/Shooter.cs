using Core.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    private GameObject bulletPrefab;
    private float bulletSpeed;
    private float attackPower;
    private float bulletScale;

    private void Start()
    {
        // 부모찾기 TODO : 오브젝트 추가
        // 부모의 data를 받아와 대입
        Buggy buggy = owner.GetComponent<Buggy>();
        if (buggy != null)
        {
            Debug.Log("buggy2");
            bulletPrefab = buggy.data.bulletPrefab;
            bulletSpeed = buggy.data.bulletSpeed;
            attackPower = buggy.data.attackPower;
            bulletScale = buggy.data.bulletScale;
            return;
        }
        PoliceWithPistol policeWithPistol = owner.GetComponent<PoliceWithPistol>();
        if (policeWithPistol != null)
        {
            Debug.Log("policeWithPistol");
            bulletPrefab = policeWithPistol.data.bulletPrefab;
            bulletSpeed = policeWithPistol.data.bulletSpeed;
            attackPower = policeWithPistol.data.attackPower;
            bulletScale = policeWithPistol.data.bulletScale;
            return;
        }
        Soldier soldier = owner.GetComponent<Soldier>();
        if (soldier != null)
        {
            Debug.Log("soldier");
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
