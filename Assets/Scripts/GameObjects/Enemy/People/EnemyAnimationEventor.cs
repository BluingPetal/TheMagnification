using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEventor : MonoBehaviour
{
    public UnityEvent OnAttack;

    public void IsOnAttack()
    {
        OnAttack?.Invoke();
    }
}
