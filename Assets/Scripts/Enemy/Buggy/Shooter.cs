using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // bullet script���� ������ ó���� �� �� �ֵ��� �������� �Բ� ����
    public void Shoot(GameObject bulletPrefab, Transform target, float attackPower)
    {
        Bullet bullet = bulletPrefab.GetComponent<Bullet>();
        bullet.attackPower = attackPower;
        bullet.target = target;
        //bulletPrefab.GetComponent<Bullet>().attackPower = attackPower;
        
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
