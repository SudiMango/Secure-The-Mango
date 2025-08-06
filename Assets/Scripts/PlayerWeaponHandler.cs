using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    private PlayerManager playerManager;
    private UIManager uIManager;

    public Gun currentGun;

    // Enable player input systems
    void Start()
    {
        playerManager = PlayerManager.getInstance();
        uIManager = UIManager.getInstance();

        playerManager.primaryFire.started += onPrimaryFireStarted;
        playerManager.secondaryFire.started += onSecondaryFireStarted;
        playerManager.reload.started += onReload;

        currentGun.onReloadStarted += onReloadStarted;
        currentGun.onReloadEnded += onReloadEnded;
    }

    void Update()
    {
        uIManager.updateMagazine(currentGun.CurrentAmmo, currentGun.Data.magazineCapacity);
        uIManager.updateTotalAmmo(WeaponManager.getInstance().totalAmmo);

        if (currentGun.IsReloading)
        {
            uIManager.updateReloadSlider(currentGun.ReloadTimer / currentGun.Data.timeToReload);
        }
    }

    // MODIFIES: currentGun
    // EFFECTS: callback function for when player tries to shoot their primary
    private void onPrimaryFireStarted(InputAction.CallbackContext context)
    {
        currentGun.onPrimaryFire();
    }

    // MODIFIES: currentGun
    // EFFECTS: callback function for when player tries to shoot their secondary
    private void onSecondaryFireStarted(InputAction.CallbackContext context)
    {
        currentGun.onSecondaryFire();
    }

    // MODIFIES: currentGun
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        currentGun.onReload();
    }

    // MODIFIES: uiManager
    // EFFECTS: event called when reload has started
    private void onReloadStarted()
    {
        uIManager.updateReloadSlider(0);
    }

    // MODIFIES: uiManager
    // EFFECTS: event called when reload has ended
    private void onReloadEnded()
    {
        uIManager.updateReloadSlider(0);
    }
}
