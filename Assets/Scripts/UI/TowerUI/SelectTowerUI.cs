using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectTowerUI : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        // UI √ ±‚»≠
        Transform childImg = transform.GetChild(0);
        Image img = childImg.GetComponent<Image>();
        int cost;
        BuildManager.Instance.FoundTower(img.sprite, out cost);
        Debug.Log(cost);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cost.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectTower();
    }

    private void SelectTower()
    {
        Transform childImg = transform.GetChild(0);
        Image img = childImg.GetComponent<Image>();

        int cost;
        BuildManager.Instance.InstantiateTower(img.sprite, out cost);
        //Debug.Log(cost);
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cost.ToString();
    }
}
