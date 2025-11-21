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
    
    public float phaseTimer; // Made public for Debug UI, keep public or use property?
    // Debug UI accesses this directly. Let's keep it public for now or make it a property.
    // User asked "possible range", so let's keep it simple for DebugUI compatibility or update DebugUI.
    // Since DebugUI is separate, let's make properties.
    
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
        // Enable interaction, disable chaser
        
        if (PlayerControl.Instance != null) {PlayerControl.Instance.SetModelVisibility(false);}
    }

    public void StartChaserPhase()
    {
        CurrentPhase = GamePhase.Chaser;
        phaseTimer = chaserPhaseDuration;
        Debug.Log("Chaser Phase Started");
        // Disable interaction, enable chaser
        if (ChaserAI.Instance != null) {ChaserAI.Instance.StartChasing();}
        
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
