using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenuUI : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.ResumeGame();
        Destroy(this.gameObject);
    }

    //public void Restart()
    //{
    //    GameManager.Instance.ChangeScene("GameScene");
    //    Destroy(this.gameObject);
    //    // TODO : �ʱ�ȭ �ʿ�
    //}

    public void ToTitle()
    {
        GameManager.Instance.ChangeScene("TitleScene");
        Destroy(this.gameObject);
    }
}
