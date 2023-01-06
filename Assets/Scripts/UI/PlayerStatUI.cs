using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI curMoneyUI;
    [SerializeField]
    private TextMeshProUGUI curLifeUI;

    private void Start()
    {
        // money�� life�� �ٲ� ������ �ڵ����� ui�� �ٲ�� ����
        PlayerStatManager.Instance.OnChangeMoney.AddListener(ChangeMoneyUI);
        PlayerStatManager.Instance.OnChangeLife.AddListener(ChangeLifeUI);
        PlayerStatManager.Instance.OnChangeMoney.Invoke();
        PlayerStatManager.Instance.OnChangeLife.Invoke();
    }

    private void ChangeMoneyUI()
    {
        curMoneyUI.text = PlayerStatManager.Instance.Money.ToString();
    }
    private void ChangeLifeUI()
    {
        curLifeUI.text = PlayerStatManager.Instance.Life.ToString();
    }
}
