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
        Buggy2 buggy2 = owner.GetComponent<Buggy2>();
        if (buggy2 != null)
        {
            Debug.Log("buggy2");
            bulletPrefab = buggy2.data.bulletPrefab;
            bulletSpeed = buggy2.data.bulletSpeed;
            attackPower = buggy2.data.attackPower;
            bulletScale = buggy2.data.bulletScale;
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
