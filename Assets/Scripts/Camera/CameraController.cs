using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ��ġ�ϰ� �Ʒ��� ������ ȭ���� ���� �ö󰡱�
    // ��ġ�ϰ� �������� �ϸ� ȭ�� ���������� �̵��ϱ�
    // �ִ� �ּ� �����ϱ�
    private bool controllable;

    [SerializeField]
    private float moveSensitivity;

    private void Start()
    {
        controllable = true;
    }

    private void Update()
    {
        Move();
    }

    public void Controllable()
    {
        controllable = true;
    }

    public void NotControllable()
    {
        controllable = false;
    }

    private void Move()
    {
        if(controllable)
        {
            if(Input.GetMouseButton(0))
            {
                // TODO : clamp �Լ� ���ؼ� �ִ� �ּ� ���� �������ֱ�
                transform.Translate(-Input.GetAxis("Mouse X") * moveSensitivity * Time.deltaTime, 0, -Input.GetAxis("Mouse Y") * moveSensitivity * Time.deltaTime, Space.World);
            }
        }
    }
}
