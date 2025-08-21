using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    private PlayerManager playerManager;

    public Gun currentGun;

    void Start()
    {
        playerManager = PlayerManager.getInstance();

        playerManager.primaryFire.started += onPrimaryFireStarted;
        playerManager.secondaryFire.started += onSecondaryFireStarted;
        playerManager.reload.started += onReload;
    }

    // MODIFIES: currentWeapon
    // EFFECTS: callback function for when player tries to shoot their primary
    private void onPrimaryFireStarted(InputAction.CallbackContext context)
    {
        currentGun.onPrimaryFire();
    }

    // MODIFIES: currentWeapon
    // EFFECTS: callback function for when player tries to shoot their secondary
    private void onSecondaryFireStarted(InputAction.CallbackContext context)
    {
        currentGun.onSecondaryFire();
    }

    // MODIFIES: currentWeapon
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        currentGun.onReload();
    }
}
