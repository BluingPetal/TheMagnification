using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 터치하고 아래로 내리면 화면은 위로 올라가기
    // 터치하고 왼쪽으로 하면 화면 오른쪽으로 이동하기
    // 최대 최소 설정하기
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
            // Input은 추후 new input system 고려
            // float deltaY = Input.GetAxis("Zoom") * zoomSpeed * Time.deltaTime;
            float deltaY = -Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
            // 최대, 최소 줌 구현
            if (((transform.localPosition.y + deltaY) < maxZoom) && ((transform.localPosition.y + deltaY) > minZoom))
            {
                transform.Translate(0, deltaY, 0, Space.World);
            }

            if (Input.GetMouseButton(0))
            {
                float deltaX = -Input.GetAxis("Mouse X") * moveSensitivity * Time.deltaTime;
                float deltaZ = -Input.GetAxis("Mouse Y") * moveSensitivity * Time.deltaTime;
                // 최대, 최소 위치 구현
                if (((transform.localPosition.x + deltaX) < maxPos.x) && ((transform.localPosition.x + deltaX) > minPos.x))
                    transform.Translate(deltaX, 0, 0, Space.World);
                if (((transform.localPosition.z + deltaZ) < maxPos.y) && ((transform.localPosition.z + deltaZ) > minPos.y))
                    transform.Translate(0, 0, deltaZ, Space.World);
            }
        }
    }
}
