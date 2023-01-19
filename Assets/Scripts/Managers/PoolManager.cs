using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingleTon<PoolManager>
{
    private Dictionary<string, Stack<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Stack<GameObject>>();
    }

    private void Start()
    {
        CreatePool();
    }

    public void CreatePool()
    {

    } 

    public GameObject Get()
    {
        return gameObject;
    }

    public void Release(GameObject obj)
    {
        
    }
}
