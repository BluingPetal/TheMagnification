using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ShopManager : SingleTon<ShopManager>
{
    [SerializeField]
    private List<ScriptableObject> ItemDataList;    // �������۰� ��ġ �������� �� �� ����
    [SerializeField]
    private ItemUI itemUIPrefab;                    // �������� ����� UI
    [SerializeField]
    private GameObject lackOfMoneyUI;               // �� ���� UI

    private Transform items;                        // �������� �����ϴ� ���� �����̳� UI

    public void Setting()
    {
        items = GameObject.Find("ShopCanvas").transform.GetChild(0).GetChild(3).GetChild(0);
    }

    public void SetUI()
    {
        for(int i = 0; i < ItemDataList.Count; i++)
        {
            if (ItemDataList[i] is ShootTowerData)  // �ӽŰ�, ����
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
        // �κ��丮�� �ֱ�
        if (ItemDataList[dataIndex] is ShootTowerData)
        {
            ShootTowerData data = ItemDataList[dataIndex] as ShootTowerData;
            if (PlayerStatManager.Instance.Money >= data.level1_cost * num) // ���� ������ ���
            {
                PlayerStatManager.Instance.MoneyChange(-data.level1_cost * num);
                InventoryManager.Instance.GetItem(data.name, ItemDataList[dataIndex], num);
            }
            else
            {
                GameObject denyobject = Instantiate(lackOfMoneyUI); // �� ���� UI
                DenyUI denyUI = denyobject.GetComponent<DenyUI>();
                denyUI.text = "Not Enough Money!";
            }
        }
    }
}
