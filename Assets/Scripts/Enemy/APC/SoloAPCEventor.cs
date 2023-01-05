using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoloAPCEventor : MonoBehaviour
{
    public UnityEvent onFinishedChangeWeapon;

    public void FinishedChangeWeapon()
    {
        onFinishedChangeWeapon?.Invoke();
    }
}
