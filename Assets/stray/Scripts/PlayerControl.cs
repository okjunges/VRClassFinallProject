using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance { get; private set; }
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject playerModel; // Reference to the visual model

    public Vector2Int gridPosition; // Kept public for GridVerify access, or make property?
    
    // Properties for external access (e.g. DebugUI)
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    public float MouseSensitivity { get { return mouseSensitivity; } set { mouseSensitivity = value; } }
    
    private string currentAnimState = "idle";
    private float verticalRotation = 0f;
    private Rigidbody rb;

    void Awake()
    {
        if (Instance == null) Instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (playerCamera != null)
        {
            // Force camera to be a child of the player
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 1.6f, 0); // Default eye height
            playerCamera.transform.localRotation = Quaternion.identity;
        }
        
        // Lock cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetModelVisibility(bool isVisible)
    {
        if (playerModel != null) {playerModel.SetActive(isVisible);}
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        UpdateGridPosition();
    }

    void HandleMouseLook()
    {
        if (playerCamera == null) return;

        // LAlt to unlock cursor and pause look
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Rotate Player (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate Camera (Pitch)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        Vector3 moveDirection = (transform.right * h + transform.forward * v).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // Movement
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? RunSpeed : MoveSpeed;
            
            // Move Character
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
        }
        else
        {
            PlayIdle();
            // Remove inertia
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void PlayIdle()
    {
        if (animator != null) {animator.SetTrigger("idle");}
    }

    public void Die()
    {
        if (animator != null) {animator.SetTrigger("death");}
        this.enabled = false;
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

    void UpdateGridPosition()
    {
        // Calculate grid position based on world position
        // Assuming 1 unit = 1 tile size, and origin at (0,0)
        // Adjust scale factor as needed
        gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }
}
