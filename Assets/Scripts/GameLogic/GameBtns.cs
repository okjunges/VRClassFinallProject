using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBtns : MonoBehaviour
{
    public GameObject gameBtns;
    public GameObject gameDetail;
    public GameObject SoundUi;

    public void OnClickStartBtn()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.StartGame();
        }
    }
    public void OnClickDetailBtn()
    {
        gameBtns.SetActive(false);
        SoundUi.SetActive(false);
        gameDetail.SetActive(true);
    }

    public void OnClickBackBtn()
    {
        gameBtns.SetActive(true);
        SoundUi.SetActive(false);
        gameDetail.SetActive(false);

    }

    public void OnClickSoundBtn()
    {
        gameBtns.SetActive(false);
        SoundUi.SetActive(true);
        gameDetail.SetActive(false);
    }
    public void OnClickRestartBtn()
    {
        SceneManager.LoadScene("DevelopMonsterScene");
    }
}