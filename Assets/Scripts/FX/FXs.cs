using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FXs : MonoBehaviour
{
    ParticleSystem system;

    private void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        Destroy(gameObject, system.main.duration);
    }
}
