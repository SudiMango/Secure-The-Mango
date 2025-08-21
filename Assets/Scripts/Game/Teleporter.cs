using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform exitTeleporter;

    private PlayerManager playerManager;
    private bool isTouchingTeleporter;

    void Start()
    {
        playerManager = PlayerManager.getInstance();
        playerManager.interact.started += teleport;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingTeleporter = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingTeleporter = false;
        }
    }

    // MODIFIES: playerManager
    // EFFECTS: teleports player to the exit teleporter instantly
    void teleport(InputAction.CallbackContext context)
    {
        if (isTouchingTeleporter)
        {
            playerManager.setPosition(exitTeleporter.position);
        }
    }


}
