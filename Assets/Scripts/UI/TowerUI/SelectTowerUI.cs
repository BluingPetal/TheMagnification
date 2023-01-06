using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectTowerUI : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectTower();
    }

    private void SelectTower()
    {
        Transform childImg = transform.GetChild(0);
        Image img = childImg.GetComponent<Image>();

        BuildManager.Instance.InstantiateTower(img.sprite);
    }
}
