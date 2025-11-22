using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GamePhase
    {
        Survivor,
        Chaser,
        GameOver
    }

    public GamePhase CurrentPhase { get; private set; }

    [SerializeField] private float survivorPhaseDuration = 60f;
    [SerializeField] private float chaserPhaseDuration = 60f;
    
    public float phaseTimer; 
    
    public float SurvivorPhaseDuration { get { return survivorPhaseDuration; } set { survivorPhaseDuration = value; } }
    public float ChaserPhaseDuration { get { return chaserPhaseDuration; } set { chaserPhaseDuration = value; } }

    void Awake()
    {
        if (Instance == null) {Instance = this;}
        else {Destroy(gameObject);}
    }

    void Start()
    {
        StartSurvivorPhase();
    }

    void Update()
    {
        if (CurrentPhase == GamePhase.Survivor)
        {
            phaseTimer -= Time.deltaTime;
            if (phaseTimer <= 0) {StartChaserPhase();}
        }
        else if (CurrentPhase == GamePhase.Chaser)
        {
            phaseTimer -= Time.deltaTime;
            if (phaseTimer <= 0) {SurvivorWin();}
        }
    }

    public void StartSurvivorPhase()
    {
        CurrentPhase = GamePhase.Survivor;
        phaseTimer = survivorPhaseDuration;
        Debug.Log("Survivor Phase Started");
        
        if (PlayerControl.Instance != null) {PlayerControl.Instance.SetModelVisibility(false);}
    }

    public void StartChaserPhase()
    {
        CurrentPhase = GamePhase.Chaser;
        phaseTimer = chaserPhaseDuration;
        Debug.Log("Chaser Phase Started");
        
        // Enable chaser
        // if (ChaserAI.Instance != null)
        // {
        //     ChaserAI.Instance.StartChasing();
        // }
        
        if (PlayerControl.Instance != null)
        {
            PlayerControl.Instance.SetModelVisibility(true);
            PlayerControl.Instance.PlayIdle();
        }
    }

    public void SkipPhase() {phaseTimer = 0;}

    public void SurvivorWin()
    {
        CurrentPhase = GamePhase.GameOver;
        Debug.Log("Survivor Wins!");
    }

    public void SurvivorLose()
    {
        CurrentPhase = GamePhase.GameOver;
        Debug.Log("Survivor Lost!");
        if (PlayerControl.Instance != null) {PlayerControl.Instance.Die();}
    }
}
