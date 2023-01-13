using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI itemNumText;

    [HideInInspector]
    public Sprite icon;
    [HideInInspector]
    private int num;

    public int Num { get { return num; } set { num = value; itemNumText.text = num.ToString(); } }

    private void Start()
    {
        itemIcon.sprite = icon;
    }


}
