using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // bullet script에서 데미지 처리를 할 수 있도록 데미지도 함께 전달
    public void Shoot(GameObject bulletPrefab, Transform target, float attackPower)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.attackPower = attackPower;
        bullet.target = target;
        //bulletPrefab.GetComponent<Bullet>().attackPower = attackPower;
        
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
