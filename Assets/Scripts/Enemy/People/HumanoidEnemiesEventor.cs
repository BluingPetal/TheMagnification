using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HumanoidEnemiesEventor : MonoBehaviour
{
    public UnityEvent OnNearAttack;
    public UnityEvent OnShoot;

    public void IsOnNearAttack()
    {
        OnNearAttack?.Invoke();
    }

    public void IsOnShoot()
    {
        OnShoot?.Invoke();
    }
}