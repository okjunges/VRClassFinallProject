using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeBar : MonoBehaviour
{

    [SerializeField] private Image timebar;

    void Update()
    {
        if (timebar != null)
        {
            if (GameManager.Instance.currentState == GameState.PlayerTurn)
            {
                timebar.fillAmount = (PlayerControl.Instance.turnTime - PlayerControl.Instance.currentTime) / PlayerControl.Instance.turnTime;
            }
            else if (GameManager.Instance.currentState == GameState.MonsterTurn)
            {
                timebar.fillAmount = (MonsterLogic.Instance.turnDuration - MonsterLogic.Instance.spendTime) / MonsterLogic.Instance.turnDuration;
            }
        }
    }
}
