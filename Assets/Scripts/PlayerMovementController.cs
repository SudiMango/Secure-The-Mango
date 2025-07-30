using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Variables

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveDir = Vector2.zero;

    [Header("Jump settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float doubleJumpMultiplier = 0.7f;
    private bool canDoubleJump = false;

    [Header("Dash settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Wall sliding settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isWallSliding = false;
    private float wallSlidingSpeed = 2f;

    // Player controls
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    // Other
    Vector3 mousePos = Vector3.zero;
    private bool facingRight = true;

    #endregion

    #region Unity callbacks

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
        if (isDashing) return;

        // Getting movement direction
        moveDir = move.ReadValue<Vector2>();

        // Making character look at the correct side
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mousePos.x < transform.position.x && facingRight)
        {
            flip();
        }
        else if (mousePos.x > transform.position.x && !facingRight)
        {
            flip();
        }

        // Constantly check for wall slide and do it if applicable
        wallSlide();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        // Move character
        float unitDir = 0;
        if (moveDir.x > 0)
        {
            unitDir = 1;
        }
        else if (moveDir.x < 0)
        {
            unitDir = -1;
        }
        rb.linearVelocityX = unitDir * moveSpeed;
    }

    #endregion

    #region Custom functions

    #region

    // EFFECTS: return true if player is grounded, otherwise return false
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // EFFECTS: return true if player is touching a wall, otherwise return false
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
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

    // MODIFIES: transform.localScale
    // EFFECTS: flips the character on the x-axis
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        facingRight = !facingRight;
    }

    #endregion

    #region Movement related functions

    // MODIFIES: rb
    // EFFECTS: makes the player jump when jump input action is performed
    private void onJump(InputAction.CallbackContext context)
    {
        if (isDashing) return;

        if (isGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else if (!isGrounded() && canDoubleJump)
        {
            rb.AddForce(Vector2.up * jumpForce * doubleJumpMultiplier, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }

    // MODIFIES: self
    // EFFECTS: makes the player dash when dash input action is performed
    private void onDash(InputAction.CallbackContext context)
    {
        if (!canDash) return;

        StartCoroutine(Dash());
    }

    // MODIFIES: self, rb
    // EFFECTS: if player can wall slide, then do so
    private void wallSlide()
    {
        if (isWalled() && !isGrounded() && moveDir.x != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Math.Clamp(rb.linearVelocityY, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
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

    #endregion

    #endregion
}
