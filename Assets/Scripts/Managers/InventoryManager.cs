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
    private Transform items;                    // 아이템을 보관하는 인벤토리 컨테이너 UI

    private Dictionary<string, AcquiredItem> InventoryDictionary; // 해당하는 key값을 찾아 처리해주도록

    private void Awake()
    {
        InventoryDictionary = new Dictionary<string, AcquiredItem>();
    }

    public void GetItem(string name, ScriptableObject scriptableData, int num)
    {
        // 같은 이름이 있을 경우 개수만 높여줌
        if(InventoryDictionary.ContainsKey(name))
        {
            AcquiredItem item = InventoryDictionary[name];
            if (item.itemNum > 0) // 아이템이 인벤토리에 위치해있는 경우
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
            // dictionary에 없었던 경우 key에 데이터를 넣어줌
            AcquiredItem acquiredItem;
            acquiredItem.itemData = scriptableData;
            acquiredItem.itemNum = num;
            acquiredItem.itemUI = itemUI;
            InventoryDictionary.Add(name, acquiredItem);
        }
        else
        {
            // dictionary에 있었던 경우 key값의 value에 num만 추가해줌
            AcquiredItem item = InventoryDictionary[name];
            item.itemNum = item.itemNum + num;
            item.itemUI = itemUI;
            item.itemUI.Num = item.itemNum;
            InventoryDictionary[name] = item;
        }
    }

    public void SellItem(string name)
    {
        // UI 설정
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

        // item이 다 소모되었을 경우
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
