using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InventoryItemSelectUI : MonoBehaviour
{
    [HideInInspector]
    public Transform owner;
    [HideInInspector]
    public string keyName;
    [HideInInspector]
    public string itemName; // 설정해주기

    private Vector3 offsetWithParent;

    private void Start()
    {
        offsetWithParent = transform.up * GameObject.Find("InventoryCanvas").transform.position.y * 0.5f;
        this.transform.position = owner.position + offsetWithParent;
    }

    private void Update()
    {
        this.transform.position = owner.position + offsetWithParent;
    }

    public void OnButtonSelect()
    {
        Destroy(this.gameObject);
        InventoryManager.Instance.UseItem(keyName);
    }

    public void OnButtonSell()
    {
        Destroy(this.gameObject);
        InventoryManager.Instance.SellItem(keyName);
    }

    public void OnButtonCancel()
    {
        Destroy(this.gameObject);
    }
}
