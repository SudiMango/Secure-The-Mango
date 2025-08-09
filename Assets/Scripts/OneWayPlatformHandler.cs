using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatformHandler : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask platformLayer;

    private GameObject currOneWayPlatform = null;

    // Player controls
    private PlayerInputActions playerControls;
    private InputAction platformDown;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Enable player input systems
    void OnEnable()
    {
        platformDown = playerControls.Player.Down;
        platformDown.Enable();
        platformDown.performed += onPlatformDown;
    }

    // Disable player input systems
    void OnDisable()
    {
        platformDown.Disable();
    }

    // EFFECTS: returns true if player is standing on one way platform, returns false otherwise
    private bool isOnPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, 0.2f, platformLayer);
        if (hit != null)
        {
            currOneWayPlatform = hit.gameObject;
            return true;
        }
        else
        {
            currOneWayPlatform = null;
            return false;
        }
    }

    // EFFECTS: makes player go down from platform when down input action is performed
    private void onPlatformDown(InputAction.CallbackContext context)
    {
        bool canProceed = isOnPlatform();
        if (canProceed)
        {
            StartCoroutine(DisableOneWayPlatformCollision());
        }
    }

    // REQUIRES: currOneWayPlatform to not be null
    // MODIFIES: playerCollider, platformCollider
    // EFFECTS: disables collision between player and platform for a small duration of time
    private IEnumerator DisableOneWayPlatformCollision()
    {
        BoxCollider2D platformCollider = currOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
