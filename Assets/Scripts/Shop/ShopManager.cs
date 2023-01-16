using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ShopManager : SingleTon<ShopManager>
{
    [SerializeField]
    private List<ScriptableObject> ItemDataList;    // 사용아이템과 배치 아이템을 살 수 있음
    [SerializeField]
    private Transform items;                        // 아이템을 보관하는 상점 컨테이너 UI
    [SerializeField]
    private ItemUI itemUIPrefab;                    // 아이템을 띄워줄 UI
    [SerializeField]
    private GameObject lackOfMoneyUI;               // 돈 부족 UI

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        for(int i = 0; i < ItemDataList.Count; i++)
        {
            if (ItemDataList[i] is ShootTowerData)  // 머신건, 로켓
            {
                ShootTowerData data = ItemDataList[i] as ShootTowerData;
                ItemUI itemUI = Instantiate(itemUIPrefab);
                itemUI.transform.SetParent(items, false);
                itemUI.index = i;
                itemUI.itemName = data.name;
                itemUI.itemSprite = data.icon;
                itemUI.itemCost = data.level1_cost;
                itemUI.attackPower = data.level1_attackPower;
                itemUI.attackRoutine = data.level1_attackRoutine;
            }
        }
    }

    public void Buy(int dataIndex, int num)
    {
        // 인벤토리에 넣기
        if (ItemDataList[dataIndex] is ShootTowerData)
        {
            ShootTowerData data = ItemDataList[dataIndex] as ShootTowerData;
            if (PlayerStatManager.Instance.Money >= data.level1_cost * num) // 구매 가능할 경우
            {
                PlayerStatManager.Instance.MoneyChange(-data.level1_cost * num);
                InventoryManager.Instance.GetItem(data.name, ItemDataList[dataIndex], num);
            }
            else
            {
                GameObject denyobject = Instantiate(lackOfMoneyUI); // 돈 부족 UI
                DenyUI denyUI = denyobject.GetComponent<DenyUI>();
                denyUI.text = "Not Enough Money!";
            }
        }
    }
}
