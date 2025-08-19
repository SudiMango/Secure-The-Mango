using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{

    #region Variables

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    [Header("Movement settings")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveDir = Vector2.zero;
    [SerializeField] private ParticleSystem footstepEffect;

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

    [Header("Wall jump settings")]
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 16f);
    private bool isWallJumping = false;
    private float wallJumpDir;
    private float wallJumpDuration = 0.4f;

    [Header("Ladder settings")]
    [SerializeField] private float ladderSpeed;
    private bool isLaddered = false;
    private bool isClimbing = false;

    // Moving platforms
    private MovingPlatform movingPlatform;
    private float movingPlatformSpeed = 0f;

    // Other
    private Vector3 mousePos = Vector3.zero;
    private bool facingRight = true;
    private bool canMove = true;
    private PlayerManager playerManager;
    private float unitDirX = 0;


    #endregion

    #region Unity callbacks

    // Enable player input systems
    void Start()
    {
        playerManager = PlayerManager.getInstance();

        playerManager.jump.started += onJump;
        playerManager.shift.started += onDash;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) return;

        // Getting movement direction
        moveDir = playerManager.move.ReadValue<Vector2>();

        // Making character look at the correct side
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (!isWallJumping)
        {
            if (mousePos.x < transform.position.x && facingRight)
            {
                flip();
            }
            else if (mousePos.x > transform.position.x && !facingRight)
            {
                flip();
            }
        }

        // Ladder
        if (isLaddered && moveDir.y > 0)
        {
            isClimbing = true;
        }

        // Moving platform
        if (movingPlatform != null)
        {
            movingPlatformSpeed = movingPlatform.moveSpeed * movingPlatform.getDir();
        }
        else
        {
            movingPlatformSpeed = 0;
        }

        // Constantly check for wall slide and do it if applicable
        wallSlide();

        // Constantly check for wall jump and do it if applicable
        wallJump();

        // Enable footstep particles
        if (Math.Abs(unitDirX) > 0 && isGrounded())
        {
            if (!footstepEffect.isEmitting)
            {
                footstepEffect.Play();
            }
        }
        else
        {
            if (footstepEffect.isEmitting)
            {
                footstepEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        if (!canMove) return;

        // Move character
        if (moveDir.x > 0)
        {
            unitDirX = 1;
        }
        else if (moveDir.x < 0)
        {
            unitDirX = -1;
        }
        else
        {
            unitDirX = 0;
        }
        if (!isWallJumping)
        {
            rb.linearVelocityX = (unitDirX * moveSpeed) + movingPlatformSpeed;
        }

        // Ladder
        if (isClimbing)
        {
            rb.gravityScale = 0;

            float unitDirY = 0;
            if (moveDir.y > 0)
            {
                unitDirY = 1;
            }
            else if (moveDir.y < 0)
            {
                unitDirY = -1;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, unitDirY * ladderSpeed);
        }
        else
        {
            rb.gravityScale = 6;
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLaddered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLaddered = false;
            isClimbing = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = collision.gameObject.GetComponent<MovingPlatform>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            movingPlatform = null;
        }
    }

    // EFFECTS: returns the direction based on player's x localscale
    private int getDir()
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

    // MODIFIES: self, rb
    // EFFECTS: makes the player jump when jump input action is performed
    private void onJump(InputAction.CallbackContext context)
    {
        if (isDashing) return;
        if (!canMove) return;

        // Normal jumping and double jumping
        if (isGrounded() && !isWallSliding && !isWallJumping)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else if (!isGrounded() && canDoubleJump && !isWallSliding && !isWallJumping)
        {
            rb.AddForce(Vector2.up * jumpForce * doubleJumpMultiplier, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

        // Wall jumping
        if (isWallSliding)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDir * wallJumpPower.x, wallJumpPower.y);
            canDoubleJump = false;

            if (transform.localScale.x != wallJumpDir)
            {
                flip();
            }

            Invoke(nameof(stopWallJump), wallJumpDuration);
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
    // EFFECTS: performs dash action
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = new Vector2(getDir() * dashForce, 0);
        float prevGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.gravityScale = prevGravityScale;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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

    // MODIFIES: self
    // EFFECTS: allows player to wall jump
    private void wallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDir = -getDir();

            CancelInvoke(nameof(stopWallJump));
        }
    }

    // MODIFIES: self
    // EFFECTS: cancels wall jump
    private void stopWallJump()
    {
        isWallJumping = false;
    }

    // MODIFIES: self
    // EFFECTS: disables player movement
    public void disableMovement()
    {
        canMove = false;
    }

    // MODIFIES: self
    // EFFECTS: enables player movement
    public void enableMovement()
    {
        canMove = true;
    }

    #endregion

    #endregion
}
