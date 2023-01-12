using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private GameObject buyUIPrefab;

    [HideInInspector]
    public int index;
    [HideInInspector]
    public string itemName;
    [HideInInspector]
    public Sprite itemSprite;
    [HideInInspector]
    public int itemCost;
    [HideInInspector]
    public float attackPower;
    [HideInInspector]
    public float attackRoutine;

    private GameObject instantiatedBuyUI;

    void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        itemNameUI.text = itemName;
        itemImgUI.sprite = itemSprite;
        itemCostUI.text = string.Format("Cost : {0}", itemCost.ToString());
        attackPowerUI.text = string.Format("Attack Power : {0}", attackPower.ToString());
        attackRoutineUI.text = string.Format("Attack Routine : {0}", attackRoutine.ToString());
    }

    public void SelectItem()
    {
        if (instantiatedBuyUI == null || instantiatedBuyUI.IsDestroyed())
        {
            instantiatedBuyUI = Instantiate(buyUIPrefab);
            BuyUI ui = instantiatedBuyUI.GetComponent<BuyUI>();
            ui.itemName = itemName;
            ui.index = index;
        }
    }
}
