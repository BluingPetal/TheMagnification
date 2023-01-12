using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopManager : SingleTon<ShopManager>
{
    [SerializeField]
    private List<ScriptableObject> ItemDataList;    // �������۰� ��ġ �������� �� �� ����
    [SerializeField]
    private Transform items;                        // �������� �����ϴ� ���� �����̳� UI
    [SerializeField]
    private ItemUI itemUIPrefab;                    // �������� ����� UI
    [SerializeField]
    private GameObject lackOfMoneyUI;               // �� ���� UI

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        for(int i = 0; i < ItemDataList.Count; i++)
        {
            if (ItemDataList[i] is ShootTowerData)  // �ӽŰ�, ����
            {
                ShootTowerData data = ItemDataList[i] as ShootTowerData;
                ItemUI itemUI = Instantiate(itemUIPrefab);
                itemUI.transform.SetParent(items);
                itemUI.index = i;
                itemUI.itemName = data.name;
                itemUI.itemSprite = data.icon;
                itemUI.itemCost = data.level1_cost;
                itemUI.attackPower = data.level1_attackPower;
                itemUI.attackRoutine = data.level1_attackRoutine;
            }
        }
    }

    public void Buy(int dataIndex)
    {
        // �κ��丮�� �ֱ�
        if (ItemDataList[dataIndex] is ShootTowerData)
        {
            ShootTowerData data = ItemDataList[dataIndex] as ShootTowerData;
            if (PlayerStatManager.Instance.Money >= data.level1_cost)
            {
                PlayerStatManager.Instance.MoneyChange(-data.level1_cost);
            }
            else
            {
                Instantiate(lackOfMoneyUI); // �� ���� UI
            }
        }
    }
}
