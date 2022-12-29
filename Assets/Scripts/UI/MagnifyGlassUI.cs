using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagnifyGlassUI : MonoBehaviour
{
    [SerializeField]
    private Camera MagnifyGlassCam;

    public void GlassControl()
    {
        if (MagnifyGlassCam.gameObject.activeSelf)
            MagnifyGlassCam.gameObject.SetActive(false);
        else
            MagnifyGlassCam.gameObject.SetActive(true);
    }

    private void OnMouseOver()
    {
        
    }
}
