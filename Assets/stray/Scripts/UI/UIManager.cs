using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Debug UI Settings")]
    [SerializeField] private bool showDebugUI = false;
    [SerializeField] private Rect windowRect = new Rect(20, 20, 250, 300);

    [Header("Interaction Prompt Settings")]
    [SerializeField] private Vector2 promptPosition = new Vector2(0.5f, 0.8f); // Screen relative
    [SerializeField] private int promptFontSize = 30;
    [SerializeField] private Color promptTextColor = Color.white;
    
    private bool showInteractionPrompt = false;
    private string interactionPromptText = "";

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Toggle Debug UI with backtick
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            showDebugUI = !showDebugUI;
            
            // Unlock cursor when UI is open
            if (showDebugUI)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Only lock if we are not holding Alt (PlayerControl logic might conflict, but this is a toggle)
                // Ideally PlayerControl manages cursor, but for DebugUI we need control.
                // Let's just set it, PlayerControl Update might override it if not careful.
                // For now, let's leave cursor logic to PlayerControl mostly, but unlock here is useful.
            }
        }
    }

    public void SetInteractionPrompt(bool isVisible, string text = "")
    {
        showInteractionPrompt = isVisible;
        interactionPromptText = text;
    }

    void OnGUI()
    {
        if (showDebugUI)
        {
            windowRect = GUI.Window(0, windowRect, DrawDebugWindow, "Game Debug");
        }

        if (showInteractionPrompt && !string.IsNullOrEmpty(interactionPromptText))
        {
            DrawInteractionPrompt();
        }
    }

    void DrawInteractionPrompt()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = promptFontSize;
        style.normal.textColor = promptTextColor;
        style.alignment = TextAnchor.MiddleCenter;
        
        GUIStyle shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = Color.black;

        float x = Screen.width * promptPosition.x;
        float y = Screen.height * promptPosition.y;
        float width = 300f;
        float height = 50f;
        
        // Draw shadow
        GUI.Label(new Rect(x - width/2 + 2, y - height/2 + 2, width, height), interactionPromptText, shadowStyle);
        // Draw text
        GUI.Label(new Rect(x - width/2, y - height/2, width, height), interactionPromptText, style);
    }

    void DrawDebugWindow(int windowID)
    {
        GUILayout.BeginVertical();

        // Game Manager Stats
        if (GameManagerSample.Instance != null)
        {
            GUILayout.Label($"Phase: {GameManagerSample.Instance.CurrentPhase}");
            GUILayout.Label($"Time Remaining: {GameManagerSample.Instance.phaseTimer:F1}s");
            
            GUILayout.Space(10);
            GUILayout.Label("Phase Settings:");
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Survivor Time:");
            string survivorTimeStr = GUILayout.TextField(GameManagerSample.Instance.SurvivorPhaseDuration.ToString(), 5);
            if (float.TryParse(survivorTimeStr, out float sTime)) GameManagerSample.Instance.SurvivorPhaseDuration = sTime;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Chaser Time:");
            string chaserTimeStr = GUILayout.TextField(GameManagerSample.Instance.ChaserPhaseDuration.ToString(), 5);
            if (float.TryParse(chaserTimeStr, out float cTime)) GameManagerSample.Instance.ChaserPhaseDuration = cTime;
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Skip Phase"))
            {
                GameManagerSample.Instance.SkipPhase();
            }
        }

        GUILayout.Space(10);

        // Player Stats
        if (PlayerControl.Instance != null)
        {
            GUILayout.Label("Player Settings:");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed:");
            PlayerControl.Instance.MoveSpeed = GUILayout.HorizontalSlider(PlayerControl.Instance.MoveSpeed, 0f, 20f);
            GUILayout.Label(PlayerControl.Instance.MoveSpeed.ToString("F1"));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Chaser Stats
        // if (ChaserAI.Instance != null)
        // {
        //     GUILayout.Label("Chaser Settings:");
        //     GUILayout.BeginHorizontal();
        //     GUILayout.Label("Speed:");
        //     ChaserAI.Instance.Speed = GUILayout.HorizontalSlider(ChaserAI.Instance.Speed, 0f, 20f);
        //     GUILayout.Label(ChaserAI.Instance.Speed.ToString("F1"));
        //     GUILayout.EndHorizontal();
        // }

        GUILayout.Space(20);
        if (GUILayout.Button("Restart Game"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        GUILayout.EndVertical();
        GUI.DragWindow();
    }
}
