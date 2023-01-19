using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGameScene : MonoBehaviour
{
    private bool startGame;
    [SerializeField]
    private Animator camAnimator;
    [SerializeField]
    private Animator UIAnimator;

    private void Start()
    {
        startGame = false;
    }

    private void Update()
    {
        if(Input.anyKeyDown && !startGame)
        {
            startGame = true;
            camAnimator.SetBool("StartGame", true);
            UIAnimator.SetBool("StartGame", true);
            StartCoroutine(GameSceneWaitTime());
        }
    }

    private IEnumerator GameSceneWaitTime()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameScene");
    }
}
