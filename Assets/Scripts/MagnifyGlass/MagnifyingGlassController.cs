using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class MagnifyingGlassController : MonoBehaviour
{
    private Vector3 initPos;
    private Animator animator;

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

    private bool Magnify
    {
        get { return animator.GetBool("Magnify"); }
        set { animator.SetBool("Magnify", value); }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
            pos.x = Mathf.Clamp(pos.x, 0+ paddingX, Screen.width - paddingX);
            pos.y = Mathf.Clamp(pos.y, 0 - paddingY, Screen.height - paddingY); // �����Ⱑ ȭ�� ������ ���� �� ������ ����
            Vector3 worldPos = new Vector3(pos.x, pos.y, locationHeight);
            Vector3 offset = new Vector3(0, -0.5f, 0);                          // ������ �κ����� ���� �� �ֵ��� ����

            transform.position = magnifyingGlassCam.ScreenToWorldPoint(worldPos) + offset;
            // ķ�� ��ġ�� �������� ��ġ�� ����
            OnGlassMoveEvent?.Invoke(magnifyingGlassCam.transform.position, glassTransform.position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Magnify = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Magnify = false;
            transform.position = initPos;                               // ��� ����� ���� �ڸ��� ����
            OnGlassMoveEvent?.Invoke(magnifyingGlassCam.transform.position, glassTransform.position);
        }
    }
}
