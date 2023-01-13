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

    // instantiate되는 window를 보유하고 있는 컨테이너를 두어 자식을 한개만 보유하도록 함
    // 창을 끄거나 다른 item ui를 클릭할 시 가지고 있는 자식을 destroy
    private Transform buyUIcontainer;

    private void Awake()
    {
        buyUIcontainer = GameObject.Find("ShopCanvas").transform.GetChild(1);
    }

    private void Start()
    {
        SetUI();
    }

    private void OnDisable()
    {
        DestroyBuyUI();
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
        DestroyBuyUI();

        GameObject instantiatedBuyUI = Instantiate(buyUIPrefab);
        instantiatedBuyUI.transform.SetParent(buyUIcontainer);
        BuyUI ui = instantiatedBuyUI.GetComponent<BuyUI>();
        ui.itemName = itemName;
        ui.index = index;
        ui.cost = itemCost;
    }

    private void DestroyBuyUI()
    {
        if(buyUIcontainer.childCount > 0)
            Destroy(buyUIcontainer.GetChild(0).gameObject);
    }
}
