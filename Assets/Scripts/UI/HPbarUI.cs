using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPbarUI : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void ChangeHPBarUI(float maxHP, float curHP)
    {
        transform.GetChild(1).GetComponent<Image>().fillAmount = curHP / maxHP;
    }
}
