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
    [SerializeField]
    private float mouseSensitivity;
    public void GlassMove(Vector3 camPos, Vector3 glassPos)
    {
        Vector3 posDiff = glassPos - camPos;
        // ���� ī�޶�� glass�� ��ġ ���� ���
        Vector3 selfPosDiff = Camera.main.transform.right * posDiff.x * mouseSensitivity + Camera.main.transform.up * posDiff.y * mouseSensitivity + Camera.main.transform.forward * zoomParameter;

        glassCam.transform.position = Camera.main.transform.position + selfPosDiff;
        Vector3 lookPos = selfPosDiff.normalized;
    }
}
