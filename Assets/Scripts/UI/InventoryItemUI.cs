using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI itemNumText;
    [SerializeField]
    private GameObject selectedItemUIPrefab;

    [HideInInspector]
    public Sprite icon;
    [HideInInspector]
    public string keyName;

    private int num;
    private Transform itemSelectedUIContainer;

    public int Num 
    { 
        get { return num; } 
        set 
        { 
            num = value;
            itemNumText.text = num.ToString();
            if (num <= 0) Destroy(this.gameObject);
        } 
    }

    private void Awake()
    {
        itemSelectedUIContainer = GameObject.Find("InventoryCanvas").transform.GetChild(1);
    }

    private void Start()
    {
        itemIcon.sprite = icon;
    }

    private void OnDisable()
    {
        DestroyItemSelectedUI();
    }

    public void InventoryItemSelected()
    {
        DestroyItemSelectedUI();

        GameObject instantiatedSelectedUI = Instantiate(selectedItemUIPrefab);
        instantiatedSelectedUI.transform.SetParent(itemSelectedUIContainer);
        InventoryItemSelectUI ui = instantiatedSelectedUI.GetComponent<InventoryItemSelectUI>();
        ui.owner = this.transform;
        ui.keyName = keyName;
        // 넘겨주어야 할 정보 입력해주기
    }

    private void DestroyItemSelectedUI()
    {
        if (itemSelectedUIContainer.childCount > 0)
            Destroy(itemSelectedUIContainer.GetChild(0).gameObject);
    }
}
