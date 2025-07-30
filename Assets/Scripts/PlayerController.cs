using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Public variables

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    // Private variables

    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;

    private Vector2 moveDir = Vector2.zero;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Enable player input systems
    void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;

        move.Enable();
        jump.Enable();

        jump.performed += onJump;
    }

    // Disable player input systems
    void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = moveDir.x * moveSpeed;
    }

    // MODIFIES: rb
    // EFFECTS: makes the player jump when jump input action is performed
    private void onJump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
