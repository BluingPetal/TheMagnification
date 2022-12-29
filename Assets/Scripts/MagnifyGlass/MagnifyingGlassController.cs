using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagnifyingGlassController : MonoBehaviour
{
    private Vector3 initPos;

    [Header("Setting")]
    [SerializeField]
    private Camera magnifyingGlassCam;
    [SerializeField]
    private Transform glassTransform;

    [Header("Position")]
    [SerializeField]
    private float paddingX = 50;
    [SerializeField]
    private float paddingY = 100;
    [SerializeField]
    private float locationHeight = 1.2f;

    public UnityEvent<Vector3, Vector3> OnGlassMoveEvent;

    private void Start()
    {
        initPos = transform.position;
        OnGlassMoveEvent?.Invoke(magnifyingGlassCam.transform.position, glassTransform.position);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            pos.x = Mathf.Clamp(pos.x, 0+ paddingX, Screen.width- paddingX);
            pos.y = Mathf.Clamp(pos.y, 0 - paddingY, Screen.height - paddingY);   // 돋보기가 화면 밖으로 나갈 수 없도록 조정
            Vector3 worldPos = new Vector3(pos.x, pos.y, locationHeight);
            Vector3 offset = new Vector3(0, -0.5f, 0);                  // 손잡이 부분으로 집을 수 있도록 조정

            transform.position = magnifyingGlassCam.ScreenToWorldPoint(worldPos) + offset;
            // 캠의 위치와 돋보기의 위치를 전달
            OnGlassMoveEvent?.Invoke(magnifyingGlassCam.transform.position, glassTransform.position);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            transform.position = initPos;                               // 사용 종료시 원래 자리로 복귀
            OnGlassMoveEvent?.Invoke(magnifyingGlassCam.transform.position, glassTransform.position);
        }
    }
}
