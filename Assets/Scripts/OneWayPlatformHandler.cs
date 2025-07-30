using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatformHandler : MonoBehaviour
{

    [SerializeField] private BoxCollider2D playerCollider;

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
        platformDown = playerControls.Player.PlatformDown;
        platformDown.Enable();
        platformDown.performed += onPlatformDown;
    }

    // Disable player input systems
    void OnDisable()
    {
        platformDown.Disable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currOneWayPlatform = null;
        }
    }

    // EFFECTS: makes player go down from platform when down input action is performed
    private void onPlatformDown(InputAction.CallbackContext context)
    {
        if (currOneWayPlatform != null)
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
