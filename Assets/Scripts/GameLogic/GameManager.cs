using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState currentState;
    public MonsterLogic monsterLogic;
    public float monsterTurnTime;
    public int roundCount;

    void Start()
    {
        currentState = GameState.PlayerTurn;
        roundCount = 1;
        // 테스트 용
        ChangeState(GameState.MonsterTurn);
    }

    void Update()
    {
        // 게임 상태에 따른 로직 처리

    }
    
    void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.PlayerTurn:
                // 플레이어 턴 시작 로직
                break;
            case GameState.MonsterTurn:
                // 몬스터 턴 시작 로직
                WallRandomRotateTest.Instance.RandomRotateWalls();   // 테스트용 벽 무작위 회전
                BakeNewMap.Instance.BakeNow();                       // 벽 바뀐 맵 다시 굽기
                monsterLogic.StartMonsterTurn(monsterTurnTime);
                break;
            case GameState.GameOver:
                // 게임 오버 로직
                break;
            case GameState.Victory:
                // 승리 로직
                break;
        }
    }
}
