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
        Debug.Log(string.Format("maxHp : {0}, curHP : {1}", maxHP, curHP));
        if(curHP > 0)
            transform.GetChild(1).GetComponent<Image>().fillAmount = curHP / maxHP;
        else
            transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
    }
}
