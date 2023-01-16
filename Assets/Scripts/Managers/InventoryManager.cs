using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

public class InventoryManager : SingleTon<InventoryManager>
{
    [SerializeField]
    private InventoryItemUI inventoryItemUIPrefab;
    [SerializeField]
    private Transform items;                    // �������� �����ϴ� �κ��丮 �����̳� UI

    private Dictionary<string, AcquiredItem> InventoryDictionary; // �ش��ϴ� key���� ã�� ó�����ֵ���

    private void Awake()
    {
        InventoryDictionary = new Dictionary<string, AcquiredItem>();
    }

    public void GetItem(string name, ScriptableObject scriptableData, int num)
    {
        // ���� �̸��� ���� ��� ������ ������
        if(InventoryDictionary.ContainsKey(name))
        {
            AcquiredItem item = InventoryDictionary[name];
            if (item.itemNum > 0) // �������� �κ��丮�� ��ġ���ִ� ���
            {
                item.itemNum = item.itemNum + num;
                item.itemUI.Num = item.itemNum;
                InventoryDictionary[name] = item;
                return;
            }
        }

        InventoryItemUI itemUI = Instantiate(inventoryItemUIPrefab);
        if (scriptableData is ShootTowerData)
        {
            ShootTowerData data = scriptableData as ShootTowerData;
            itemUI.transform.SetParent(items, false);
            itemUI.icon = data.icon;
            itemUI.Num = num;
            itemUI.keyName = name;
        }
        
        if (!InventoryDictionary.ContainsKey(name))
        {
            // dictionary�� ������ ��� key�� �����͸� �־���
            AcquiredItem acquiredItem;
            acquiredItem.itemData = scriptableData;
            acquiredItem.itemNum = num;
            acquiredItem.itemUI = itemUI;
            InventoryDictionary.Add(name, acquiredItem);
        }
        else
        {
            // dictionary�� �־��� ��� key���� value�� num�� �߰�����
            AcquiredItem item = InventoryDictionary[name];
            item.itemNum = item.itemNum + num;
            item.itemUI = itemUI;
            item.itemUI.Num = item.itemNum;
            InventoryDictionary[name] = item;
        }
    }

    public void SellItem(string name)
    {
        // UI ����
        AcquiredItem item = InventoryDictionary[name];

        if (InventoryDictionary[name].itemNum > 0)
        {
            InventoryDictionary[name].itemUI.Num = --item.itemNum;
            InventoryDictionary[name] = item;
            ScriptableObject objData = InventoryDictionary[name].itemData;
            if (objData is ShootTowerData)
            {
                ShootTowerData data = objData as ShootTowerData;
                PlayerStatManager.Instance.MoneyChange((int)(data.level1_cost * 0.5f));
            }
        }

        // item�� �� �Ҹ�Ǿ��� ���
        if (InventoryDictionary[name].itemNum <= 0)
        {
            InventoryDictionary[name].itemUI.Num = 0;
        }
    }


    public struct AcquiredItem
    {
        public ScriptableObject itemData;
        public int itemNum;
        public InventoryItemUI itemUI;
    }
}
