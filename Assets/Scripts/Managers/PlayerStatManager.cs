using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatManager : SingleTon<PlayerStatManager>
{
    // 돈, 목숨, 인벤토리 등 스텟 관련된 사항들
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
        private set { money = value; OnChangeMoney.Invoke(); } // UI 바꿔주고, 살 수 있는 타워 추가해주기
    }

    public int Life
    {
        get { return life; }
        private set { life = value; OnChangeLife.Invoke(); } // UI 바꿔주고, 살 수 있는 타워 추가해주기
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