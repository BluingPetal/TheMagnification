using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;

    [HideInInspector]
    public string itemName;
    [HideInInspector]
    public int index;

    private void Start()
    {
        nameText.text = string.Format("({0})", itemName);
    }

    public void CancelItem()
    {
        Destroy(this.gameObject);
    }

    public void BuyItem()
    {
        ShopManager.Instance.Buy(index);
        Destroy(this.gameObject);
    }
}
