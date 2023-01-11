using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI itemNameUI;
    [SerializeField]
    protected Image itemImgUI;
    [SerializeField]
    protected TextMeshProUGUI itemCostUI;
    [SerializeField]
    protected TextMeshProUGUI attackPowerUI;
    [SerializeField]
    protected TextMeshProUGUI attackRoutineUI;

    protected string itemName;
    protected Sprite itemSprite;
    protected int itemCost;
    protected int attackPower;
    protected int attackRoutine;

    void Start()
    {
        SetData();
        SetUI();
    }

    virtual protected void SetData() { }

    private void SetUI()
    {
        itemNameUI.text = itemName;
        itemImgUI.sprite = itemSprite;
        itemCostUI.text = string.Format("Cost : {0}", itemCost.ToString());
        attackPowerUI.text = string.Format("Attack Power : {0}", attackPower.ToString());
        attackRoutineUI.text = string.Format("Attack Routine : {0}", attackRoutine.ToString());
    }
}
