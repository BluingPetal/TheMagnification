using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Place : MonoBehaviour
{
    private Renderer rend;

    [Header("Material")]
    // �Է¿� �������� ��� material ����
    [SerializeField]
    protected Material mouseEnterMat;
    [SerializeField]
    protected Material mouseExitMat;

    public bool isOccupied;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        isOccupied = false;
    }

    private void OnMouseEnter()
    {
        BuildManager.Instance.selectedPlace = this;
        rend.material = mouseEnterMat;
    }

    private void OnMouseExit()
    {
        BuildManager.Instance.selectedPlace = null;
        rend.material = mouseExitMat;
    }

    //private void OnMouseUpAsButton() // ���߿� �����
    //{
    //    BuildManager.Instance.Build();
    //}
}
