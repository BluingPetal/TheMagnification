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
                // TODO : clamp 함수 통해서 최대 최소 범위 생성해주기
                transform.Translate(-Input.GetAxis("Mouse X") * moveSensitivity * Time.deltaTime, 0, -Input.GetAxis("Mouse Y") * moveSensitivity * Time.deltaTime, Space.World);
            }
        }
    }
}
