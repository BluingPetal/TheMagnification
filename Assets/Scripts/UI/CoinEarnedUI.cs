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

    private Transform playerStatUITransform;

    private void Awake()
    {
        playerStatUITransform = GameObject.Find("PlayerStatPanel").transform;
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
        transform.position = playerStatUITransform.position + new Vector3(200, 0, 0);
    }

    private void Update()
    {
        coinText.transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        coinText.color -= new Color(0, 0, 0, alphaSpeed * Time.deltaTime); 
    }

    public void ChangeCoinText(int coin)
    {
        if (coin > 0)
        {
            coinText.color = Color.yellow;
            coinText.text = string.Format("+ {0}", coin);
        }
        else
        {
            coinText.color = Color.red;
            coinText.text = string.Format("- {0}", Mathf.Abs(coin));
        }
    }
}
