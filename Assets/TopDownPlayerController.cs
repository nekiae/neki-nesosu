using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 15f;
    public float deceleration = 50f;

    public GameObject textUI;

    [Header("Animation")]
    public Vector3 tableCoord = new(3.03f, 0.53f, -0.1f);

    [Header("Dialogs")]
    public TextController dialogController;


    private NavMeshAgent navigationAgent;

    private bool nearTable = false;

    private Vector2 moveInput;
    private Vector2 currentVelocity;


    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private TextMeshProUGUI pressE;

    private PlayerInput playerInput;

    private Animator animator;
    public void Start()
    {
        // Get required components
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pressE = textUI.GetComponent<TextMeshProUGUI>();
        playerInput = FindFirstObjectByType<PlayerInput>();
        navigationAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //Setup navigation agent
        navigationAgent.updateRotation = false;
        navigationAgent.updateUpAxis = false;

    }

    public void FixedUpdate()
    {
        HandleMovement();
        HandelAnimator();
    }



    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (nearTable && ctx.started)
        {
            playerInput.enabled = false;
            pressE.SetText("");
            StartCoroutine(Interaction());


        }
    }

    public void HandelAnimator()
    {
        int x;
        int y;
        if (navigationAgent.velocity != Vector3.zero)
        {
            x = System.Math.Sign(navigationAgent.velocity.x);
            y = System.Math.Sign(navigationAgent.velocity.y);
        }

        else
        {
            x = System.Math.Sign(rb.linearVelocityX);
            y = System.Math.Sign(rb.linearVelocityY);
        }


        if (y == 0)
        {
            if (x < 0)
            {
                sprite.flipX = true;
            }
            else if (x > 0)
            {
                sprite.flipX = false;
            }
        }
        else
        {
            sprite.flipX = false;
        }
        animator.SetInteger("x", x);
        animator.SetInteger("y", y);
    }

    void HandleMovement()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;

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

    IEnumerator Interaction()
    {
        yield return StartCoroutine(Move(tableCoord));
        dialogController.ShowDialog("Вочы зачыняются...");
    }

    IEnumerator Move(Vector3 endPos)
    {
        navigationAgent.SetDestination(endPos);
        while (navigationAgent.velocity == Vector3.zero)
        {
            yield return new WaitForFixedUpdate();
        }
        while (navigationAgent.velocity != Vector3.zero)
        {
            yield return new WaitForFixedUpdate();
        }

    }

}