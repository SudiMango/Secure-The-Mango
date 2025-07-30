using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveDir = Vector2.zero;

    [Header("Jump settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpMultiplier = 0.7f;
    private bool isGrounded = true;
    private bool canDoubleJump = false;


    [Header("Dash settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    // Player controls
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Enable player input systems
    void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        dash = playerControls.Player.Dash;

        move.Enable();
        jump.Enable();
        dash.Enable();

        jump.performed += onJump;
        dash.performed += onDash;
    }

    // Disable player input systems
    void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocityX = moveDir.x * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = false;
        }
    }

    // MODIFIES: rb
    // EFFECTS: makes the player jump when jump input action is performed
    private void onJump(InputAction.CallbackContext context)
    {
        if (isDashing) return;

        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else if (!isGrounded && canDoubleJump)
        {
            rb.AddForce(Vector2.up * jumpForce * doubleJumpMultiplier, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }

    // MODIFIES: rb
    // EFFECTS: makes the player dash when dash input action is performed
    private void onDash(InputAction.CallbackContext context)
    {
        if (!canDash) return;

        StartCoroutine(Dash());
    }

    // MODIFIES: self, rb
    // EFFECTS: performs dash action
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = new Vector2(getDashDir() * dashForce, 0);
        float prevGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.gravityScale = prevGravityScale;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // EFFECTS: returns the dash direction based on player's x localscale
    private int getDashDir()
    {
        if (gameObject.transform.localScale.x < 0)
        {
            return -1;
        }
        return 1;
    }
}
