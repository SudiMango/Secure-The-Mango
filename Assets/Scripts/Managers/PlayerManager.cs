using UnityEngine;
using UnityEngine.InputSystem;

// PlayerManager manages global player stuff and player input
public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameObject player;

    // Player controls
    private PlayerInputActions playerControls;

    // Movement related
    public InputAction move;
    public InputAction jump;
    public InputAction shift;
    public InputAction interact;

    public InputAction up;
    public InputAction down;

    // Weapon related
    public InputAction primaryFire;
    public InputAction secondaryFire;
    public InputAction reload;

    protected override void Awake()
    {
        base.Awake();

        // Assign unassigned variables
        playerControls = new PlayerInputActions();
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        interact = playerControls.Player.Interact;
        shift = playerControls.Player.Shift;
        up = playerControls.Player.Up;
        down = playerControls.Player.Down;
        primaryFire = playerControls.Player.PrimaryFire;
        secondaryFire = playerControls.Player.SecondaryFire;
        reload = playerControls.Player.Reload;
    }

    // Enable player input systems
    void OnEnable()
    {
        move.Enable();
        jump.Enable();
        shift.Enable();
        interact.Enable();
        up.Enable();
        down.Enable();
        primaryFire.Enable();
        secondaryFire.Enable();
        reload.Enable();
    }

    // Disable player input systems
    void OnDisable()
    {
        move.Disable();
        jump.Disable();
        shift.Disable();
        interact.Disable();
        up.Disable();
        down.Disable();
        primaryFire.Disable();
        secondaryFire.Disable();
        reload.Disable();
    }

    // EFFECTS: returns the position of the player in the world
    public Vector2 getPosition()
    {
        return player.transform.position;
    }

    // MODIFIES: player
    // EFFECTS: sets the position of the player in the world
    public void setPosition(Vector2 position)
    {
        player.transform.position = position;
    }
}
