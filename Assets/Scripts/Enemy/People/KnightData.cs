using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Knight")]
public class KnightData : MonoBehaviour
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float attackRange;
    public float attackRoutine;
    public float attackPower;

    [Header("BulletSetting")]
    public float bulletSpeed;
    public float bulletScale;
    // ��� ���������� �������� ���ϱ�
}
