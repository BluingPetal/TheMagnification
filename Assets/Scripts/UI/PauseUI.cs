using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pausedMenuUI;

    public void Pause()
    {
        Instantiate(pausedMenuUI);
        GameManager.Instance.PauseGame();
    }
}
