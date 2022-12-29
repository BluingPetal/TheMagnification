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
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private Vector2 maxPos;
    [SerializeField]
    private Vector2 minPos;
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float minZoom;

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
        if (controllable)
        {
            // Input�� ���� new input system ���
            // float deltaY = Input.GetAxis("Zoom") * zoomSpeed * Time.deltaTime;
            float deltaY = -Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            // �ִ�, �ּ� �� ����
            if (((transform.localPosition.y + deltaY) < maxZoom) && ((transform.localPosition.y + deltaY) > minZoom))
            {
                transform.Translate(0, deltaY, 0, Space.World);
            }

            if (Input.GetMouseButton(0))
            {
                float deltaX = -Input.GetAxis("Mouse X") * moveSensitivity * Time.deltaTime;
                float deltaZ = -Input.GetAxis("Mouse Y") * moveSensitivity * Time.deltaTime;
                // �ִ�, �ּ� ��ġ ����
                if (((transform.localPosition.x + deltaX) < maxPos.x) && ((transform.localPosition.x + deltaX) > minPos.x))
                    transform.Translate(deltaX, 0, 0, Space.World);
                if (((transform.localPosition.z + deltaZ) < maxPos.y) && ((transform.localPosition.z + deltaZ) > minPos.y))
                    transform.Translate(0, 0, deltaZ, Space.World);
            }
        }
    }
}
