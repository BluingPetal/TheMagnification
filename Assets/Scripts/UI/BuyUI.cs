using TMPro;
using UnityEngine;

public class BuyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameCostText;
    [SerializeField]
    private TextMeshProUGUI numText;

    [HideInInspector]
    public string itemName;
    [HideInInspector]
    public int index;
    [HideInInspector]
    public int cost;

    private int itemNum;
    private int ItemNum
    { get { return itemNum; } set { itemNum = value; numText.text = itemNum.ToString(); } }


    private void Start()
    {
        nameCostText.text = string.Format("({0}) : {1}G", itemName, cost);
        ItemNum = 1;
    }

    public void CancelItem()
    {
        Destroy(this.gameObject);
    }

    public void IncreaseNum()
    {
        if (ItemNum >= 99)
            ItemNum = 99;
        else
            ItemNum++;
    }

    public void DecreaseNum()
    {
        if (ItemNum <= 1)
            ItemNum = 1;
        else
            ItemNum--;
    }

    public void BuyItem()
    {
        ShopManager.Instance.Buy(index, ItemNum);
        Destroy(this.gameObject);
    }
}
