using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    private PlayerManager playerManager;

    public Weapon currentWeapon;

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
        currentWeapon.onPrimaryFire();
    }

    // MODIFIES: currentWeapon
    // EFFECTS: callback function for when player tries to shoot their secondary
    private void onSecondaryFireStarted(InputAction.CallbackContext context)
    {
        currentWeapon.onSecondaryFire();
    }

    // MODIFIES: currentWeapon
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        if (currentWeapon is Gun gun)
        {
            gun.onReload();
        }
    }
}
