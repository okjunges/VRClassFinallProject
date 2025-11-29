using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBtns : MonoBehaviour
{
    public GameObject gameBtns;
    public GameObject gameDetail;
    public GameObject SoundUi;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;   // 커서 잠금 해제
            Cursor.visible = true; 
            OnClickSoundBtn();
        }
    }

    public void OnClickStartBtn()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            if (SoundUi.activeSelf)
            {
                OnClickSoundBackBtn();
            }
            gm.StartGame();
        }
    }
    public void OnClickDetailBtn()
    {
        gameBtns.SetActive(false);
        SoundUi.SetActive(false);
        gameDetail.SetActive(true);
    }

    public void OnClickDetailBackBtn()
    {
        gameBtns.SetActive(true);
        gameDetail.SetActive(false);

    }

    public void OnClickSoundBackBtn()
    {
        SoundUi.SetActive(false);
        if (GameManager.Instance.currentState == GameState.PlayerTurn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void OnClickSoundBtn()
    {
        SoundUi.SetActive(true);
    }

    public void OnClickRestartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }
}