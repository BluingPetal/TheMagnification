using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DenyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI denyText;

    [HideInInspector]
    public string text;

    private void Start()
    {
        denyText.text = text;
        Destroy(gameObject, 2f);
    }
}
