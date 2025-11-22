using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public float monsterTurnTime;
    public float playerTurnTime;
    public int maxRound;
    public int roundCount;
    public GameObject UI;
    public GameObject baseUI;
    public GameObject gameOverUI;
    public GameObject victoryUI;

    void Start()
    {
        currentState = GameState.None;
        ChangeState(currentState);
        roundCount = 1;
        UI.SetActive(true);
        baseUI.SetActive(true);
        gameOverUI.SetActive(false);
        victoryUI.SetActive(false);
    }

    void Update()
    {
        // 게임 시작 전이거나, 게임 오버 또는 승리 상태일 때는 상태 전환 로직을 실행하지 않음
        if (currentState == GameState.None || currentState == GameState.GameOver || currentState == GameState.Victory) return;
        // 플레이어 턴일 때, 플레이어 턴이 끝났으면 몬스터 턴으로 전환
        if (currentState == GameState.PlayerTurn && !PlayerControl.Instance.isMyTurn)
        {
            currentState = GameState.MonsterTurn;
            ChangeState(currentState);
            return;
        }
        // 몬스터 턴일 때, 턴이 끝났으면 플레이어 턴으로 전환
        if (MonsterLogic.Instance.TurnFinished && currentState == GameState.MonsterTurn)
        {
            UI.SetActive(true);
            // 몬스터가 플레이어를 발견했으면 게임 오버로 전환
            if (MonsterLogic.Instance.FoundPlayer)
            {
                currentState = GameState.GameOver;
                ChangeState(currentState);
                return;
            }
            ++roundCount;
            // 6라운드가 되면 승리 상태로 전환
            if (roundCount >= maxRound + 1)
            {
                currentState = GameState.Victory;
            }
            else
            {
                currentState = GameState.PlayerTurn;
            }
            ChangeState(currentState);
            return;
        }
    }
    
    public void StartGame()
    {
        baseUI.SetActive(false);
        roundCount = 1;
        currentState = GameState.PlayerTurn;
        ChangeState(currentState);
    }
    
    void ChangeState(GameState newState)
    {
        currentState = newState;
        AudioController.Instance.ChangeBGM(currentState); // BGM 변경
        AudioController.Instance.StopHeartbeatSound(); // 심장 소리 정지
        switch (currentState)
        {
            case GameState.None:
                // 초기 상태 로직
                MonsterLogic.Instance.OffCamera(); // 몬스터 카메라 비활성화
                PlayerControl.Instance.OnCamera(); // 플레이어 카메라 활성화
                break;
            case GameState.PlayerTurn:
                // 플레이어 턴 시작 로직
                AudioController.Instance.PlayHeartbeatSound(); // 심장 소리 실행
                MonsterLogic.Instance.OffCamera(); // 몬스터 카메라 비활성화
                PlayerControl.Instance.OnCamera(); // 플레이어 카메라 활성화
                PlayerControl.Instance.StartPlayerTurn(playerTurnTime);
                break;
            case GameState.MonsterTurn:
                // 몬스터 턴 시작 로직
                UI.SetActive(false);
                PlayerControl.Instance.OffCamera(); // 플레이어 카메라 비활성화
                MonsterLogic.Instance.OnCamera(); // 몬스터 카메라 활성화
                BakeNewMap.Instance.BakeNow();                       // 벽 바뀐 맵 다시 굽기
                MonsterLogic.Instance.StartMonsterTurn(monsterTurnTime);
                break;
            case GameState.GameOver:
                // 게임 오버 로직
                gameOverUI.SetActive(true);
                UIManager.Instance.SetInteractionPrompt(false, "");
                PlayerControl.Instance.OffCamera(); // 플레이어 카메라 비활성화
                MonsterLogic.Instance.OnCamera(); // 몬스터 카메라 활성화
                Debug.Log("플레이어 패배");
                break;
            case GameState.Victory:
                // 승리 로직
                victoryUI.SetActive(true);
                UIManager.Instance.SetInteractionPrompt(false, "");
                MonsterLogic.Instance.OffCamera(); // 몬스터 카메라 비활성화
                PlayerControl.Instance.OnCamera(); // 플레이어 카메라 활성화
                Debug.Log("플레이어 승리!");
                break;
        }
    }
}