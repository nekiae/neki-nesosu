using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float acceleration = 15f;
    [SerializeField] public float deceleration = 50f;
    
    [SerializeField] public GameObject textUI;

    [Header("Anim")]

    [SerializeField] public Vector3 tableCoord = new(3.03f, 0.53f, -0.1f);

    private bool nearTable = false;

    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;

    private TextMeshProUGUI pressE;

    private PlayerInput playerInput;
    
    public void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        
        pressE = textUI.GetComponent<TextMeshProUGUI>();

        playerInput = FindFirstObjectByType<PlayerInput>();
        

    }

    public void FixedUpdate()
    {
        HandleMovement();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

    }
    
    public void OnInteract(InputAction.CallbackContext _){
        if (nearTable){
            playerInput.enabled = false;
            StartCoroutine(Move(gameObject.transform.position, tableCoord, 1));
        }
    }

    void HandleMovement()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;
        
        // Smooth acceleration/deceleration
        if (moveInput.magnitude > 0)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration);
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration);
        }
        
        // Apply movement
        rb.linearVelocity = currentVelocity;
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "SleepTable")
        {
            nearTable = true;
            pressE.SetText("Press E");
        }
    }

        public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "SleepTable")
        {
            nearTable = false;
            pressE.SetText("");
        }
    }

    IEnumerator Move(Vector3 beginPos, Vector3 endPos, float time){
    for(float t = 0; t < 1; t += Time.deltaTime / time){
        moveInput = new Vector2(0,0);
        transform.position = Vector3.Lerp(beginPos, endPos, t);
        yield return null;
    }
}

}