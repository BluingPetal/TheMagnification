using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : SingleTon<PlayerStatManager>
{
    // ��, ���, �κ��丮 �� ���� ���õ� ���׵�
    private int money = 1000;
    private int life = 300;

    [HideInInspector]
    public UnityEvent OnChangeMoney;
    [HideInInspector]
    public UnityEvent OnChangeLife;

    [SerializeField]
    private GameObject coinEarnedUI;

    public int Money
    {
        get { return money; }
        private set { money = value; OnChangeMoney.Invoke(); } // UI �ٲ��ְ�, �� �� �ִ� Ÿ�� �߰����ֱ�
    }

    public int Life
    {
        get { return life; }
        private set { life = value; OnChangeLife.Invoke(); } // UI �ٲ��ְ�, �� �� �ִ� Ÿ�� �߰����ֱ�
    }

    private void Start()
    {
        Money = 1000;
        Life = 300;
    }

    public void MoneyChange(int money)
    {
        Money += money;
        GameObject moneyText = Instantiate(coinEarnedUI);
        moneyText.GetComponent<CoinEarnedUI>().ChangeCoinText(money);
    }
}