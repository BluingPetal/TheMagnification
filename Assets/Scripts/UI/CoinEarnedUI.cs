using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinEarnedUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;
    [SerializeField]
    private float upSpeed;
    [SerializeField]
    private float alphaSpeed;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        coinText.transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        coinText.color -= new Color(0, 0, 0, alphaSpeed * Time.deltaTime); 
    }

    public void ChangeCoinText(int coin)
    {
        coinText.color = Color.yellow;
        Debug.Log(coin);
        if (coin > 0)
            coinText.text = string.Format("+ {0}", coin);
        else
            coinText.text = string.Format("- {0}", Mathf.Abs(coin));
    }
}
