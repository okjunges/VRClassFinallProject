using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeBar : MonoBehaviour
{

    [SerializeField] private Image timebar;
    private GameManager gm;
    private float currentTime;
    private float maxTime;
    private GameState lastState = GameState.None;

    void Start()
    {
        gm = GameManager.Instance;
    }

    void OnEnable()
    {
        lastState = GameState.None;
    }

    void Update()
    {
        if (gm == null) return;

        // 상태 변경 감지
        if (gm.currentState != lastState)
        {
            Debug.Log($"[TimeBar] State changed from {lastState} to {gm.currentState}");
            lastState = gm.currentState;
            InitializeTimer();
        }


        // 타이머 로직
        if (maxTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;

            if (timebar != null)
            {
                timebar.fillAmount = currentTime / maxTime;
            }
        }
        else
        {
             if (timebar != null) timebar.fillAmount = 0;
        }
    }

    void InitializeTimer()
    {
        if (gm.currentState == GameState.PlayerTurn)
        {
            if (PlayerControl.Instance != null)
            {
                maxTime = PlayerControl.Instance.turnTime;
                currentTime = maxTime;
            }
        }
        else if (gm.currentState == GameState.MonsterTurn)
        {
            if (MonsterLogic.Instance != null)
            {
                maxTime = MonsterLogic.Instance.turnDuration;
                currentTime = maxTime;
            }
        }
        else
        {
            maxTime = 0;
            currentTime = 0;
        }
        Debug.Log($"[TimeBar] Initialized. State: {gm.currentState}, MaxTime: {maxTime}, CurrentTime: {currentTime}");
    }
}
