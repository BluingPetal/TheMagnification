using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Knight")]
public class KnightData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public string description;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stats")]
    public float hp;
    public float speed;
    public float runSpeed;
    public float attackRange;
    public float attackRoutine;
    public float attackPower;

    // ��� ���������� �������� ���ϱ�
}
