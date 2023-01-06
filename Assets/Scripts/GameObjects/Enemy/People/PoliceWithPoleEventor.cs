using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.Events;

public class PoliceWithPoleEventor : MonoBehaviour
{
    public UnityEvent OnAttack;

    public void IsOnAttack()
    {
        OnAttack?.Invoke();
    }
}
