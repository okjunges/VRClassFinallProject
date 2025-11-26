using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public GameState currentState;
    public MonsterLogic monsterLogic;
    public float monsterTurnTime;
    public int roundCount;

    public int maxRound;
    public float maxTime =60f;
    public float currentTime;

    public List<GameObject> mapPrefabs;
    public GameObject player;
    public GameObject enemy;
    private GameObject currentMapInstance;

    public static GM Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentState = GameState.PlayerTurn;
        roundCount = 1;
        currentTime = maxTime; // Initialize time
        
        if (mapPrefabs == null)
        {
            Debug.LogError("Map Prefabs not assigned in GM Inspector!");
            mapPrefabs = new List<GameObject>();
        }

        if (mapPrefabs.Count > 0)
        {
            maxRound = mapPrefabs.Count;
            currentMapInstance = Instantiate(mapPrefabs[0]);
            MovePlayerToSpawnPoint();
        }

        // 테스트 용
        ChangeState(GameState.MonsterTurn);
    }

    void Update()
    {
        if (currentState == GameState.PlayerTurn || currentState == GameState.MonsterTurn)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                // Time over logic
                if (currentState == GameState.PlayerTurn)
                {
                    ChangeState(GameState.MonsterTurn);
                }
                else if (currentState == GameState.MonsterTurn)
                {
                    ChangeState(GameState.Victory);
                }
            }
        }

        if (monsterLogic != null && monsterLogic.FoundPlayer && !(currentState == GameState.GameOver))
        {
            ChangeState(GameState.GameOver);
        }
    }
    
    void ChangeState(GameState newState)
    {
        currentState = newState;
        currentTime = maxTime; // Reset time on state change
        
        if (AudioController.Instance != null)
            AudioController.Instance.ChangeBGM(currentState); // BGM 변경

        switch (currentState)
        {
            case GameState.PlayerTurn:
                // 플레이어 턴 시작
                break;
            case GameState.MonsterTurn:
                // 몬스터 턴 시작
                if (BakeNewMap.Instance != null)
                    BakeNewMap.Instance.BakeNow(); // 벽 바뀐 맵 다시 굽기
                
                if (monsterLogic != null)
                    monsterLogic.StartMonsterTurn(monsterTurnTime);
                break;
            case GameState.GameOver:
                // 게임 오버
                break;
            case GameState.Victory:
                // 승리(라운드 클리어)
                if (roundCount < maxRound)
                {
                    roundCount++;
                    if (currentMapInstance != null) Destroy(currentMapInstance);

                    if (mapPrefabs != null && roundCount <= mapPrefabs.Count)
                    {
                        currentMapInstance = Instantiate(mapPrefabs[roundCount - 1]);
                        MovePlayerToSpawnPoint();
                        MoveEnemyToSpawnPoint();
                    }

                    ChangeState(GameState.PlayerTurn);
                }
                else
                {
                    // Real Game Victory
                    Debug.Log("All Rounds Cleared! Victory!");
                }
                break;
        }
    }

    void MovePlayerToSpawnPoint()
    {
        if (player == null) return;
        
        Vector3 targetPos = new Vector3(1.5f, 0f, 1.5f); // Default position

        if (currentMapInstance != null)
        {
            Transform spawnPoint = currentMapInstance.transform.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                targetPos = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            }
        }

        player.transform.position = targetPos;
    }
    void MoveEnemyToSpawnPoint()
    {
        if (enemy == null) return;
        
        Vector3 targetPos = new Vector3(1.5f, 0f, 1.5f); // Default position

        if (currentMapInstance != null)
        {
            Transform spawnPoint = currentMapInstance.transform.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                targetPos = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
            }
        }

        enemy.transform.position = targetPos;
    }
}
