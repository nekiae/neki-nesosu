using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 50f;
    
    [SerializeField] private GameObject textUI;
    
    
    // Private variables
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;

    private TextMeshProUGUI pressE;

    
    public void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        
        pressE = textUI.GetComponent<TextMeshProUGUI>();

    }

    public void Update()
    {
        HandleInput();
    }

    public void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        var inputActions = FindFirstObjectByType<UnityEngine.InputSystem.PlayerInput>();
        var moveAction = inputActions.actions["Move"];
        moveInput = moveAction.ReadValue<Vector2>();

    }
    
    void HandleMovement()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;
        
        // Smooth acceleration/deceleration
        if (moveInput.magnitude > 0)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }
        
        // Apply movement
        rb.linearVelocity = currentVelocity;
    }
    
    // Public methods for external access
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0, speed);
    }
    
    
    public Vector2 GetCurrentVelocity()
    {
        return currentVelocity;
    }
    
    public bool IsMoving()
    {
        return currentVelocity.magnitude > 0.1f;
    }
    
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
    
    // For animation controllers
    public float GetMoveSpeed()
    {
        return currentVelocity.magnitude;
    }
    
    public Vector2 GetMoveDirection()
    {
        return currentVelocity.normalized;
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "SleepTable")
        {
            pressE.SetText("Press E");
        }
    }

        public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "SleepTable")
        {
            pressE.SetText("");
        }
    }
}