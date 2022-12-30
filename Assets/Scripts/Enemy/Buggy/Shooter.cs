using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    // bullet script���� ������ ó���� �� �� �ֵ��� �������� �Բ� ����
    public void Shoot(GameObject bulletPrefab, float attackPower)
    {
        bulletPrefab.GetComponent<Bullet>().attackPower = attackPower;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
