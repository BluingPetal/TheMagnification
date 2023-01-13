using System.Collections;
using System.Collections.Generic;
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
            item.itemNum = item.itemNum + num;
            item.itemUI.Num = item.itemNum;
            return;
        }

        InventoryItemUI itemUI = Instantiate(inventoryItemUIPrefab);
        if (scriptableData is ShootTowerData)
        {
            ShootTowerData data = scriptableData as ShootTowerData;
            itemUI.transform.SetParent(items, false);
            itemUI.icon = data.icon;
            itemUI.Num = num;
        }

        AcquiredItem acquiredItem;
        acquiredItem.itemData = scriptableData;
        acquiredItem.itemNum = num;
        acquiredItem.itemUI = itemUI;
        InventoryDictionary.Add(name, acquiredItem);
    }


    public struct AcquiredItem
    {
        public ScriptableObject itemData;
        public int itemNum;
        public InventoryItemUI itemUI;
    }
}
