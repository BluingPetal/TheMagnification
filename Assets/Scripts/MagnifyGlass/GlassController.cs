using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class GlassController : MonoBehaviour
{
    [SerializeField]
    private Camera glassCam;
    [SerializeField]
    private float zoomParameter;
    public void GlassMove(Vector3 camPos, Vector3 glassPos)
    {
        Vector3 posDiff = glassPos - camPos;
        // ���� ī�޶�� glass�� ��ġ ���� ���
        Vector3 selfPosDiff = transform.forward * posDiff.z + transform.right * posDiff.x + transform.up * posDiff.y;

        transform.position = Camera.main.transform.position + selfPosDiff;
        glassCam.transform.position = transform.position + transform.forward * zoomParameter;
    }
}
