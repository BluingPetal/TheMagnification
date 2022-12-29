using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MagnifyGlassUI : MonoBehaviour
{
    public UnityEvent glassOnEvent;
    public UnityEvent glassOffEvent;

    [SerializeField]
    private Camera MagnifyGlassCam;

    public void GlassControl()
    {
        if (MagnifyGlassCam.gameObject.activeSelf)
        {
            MagnifyGlassCam.gameObject.SetActive(false);
            glassOffEvent?.Invoke();
        }
        else
        {
            MagnifyGlassCam.gameObject.SetActive(true);
            glassOnEvent?.Invoke();
        }
    }

    private void OnMouseOver()
    {
        
    }
}
