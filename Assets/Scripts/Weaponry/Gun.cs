using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] protected WeaponDataScriptableObject data;
    [SerializeField] protected Transform firePoint;

    // Other required variables
    protected int currentAmmo;
    protected bool isReloading = false;
    protected bool inputFrozen = false;

    protected float primaryTimer = 0f;
    protected float secondaryTimer = 0f;
    protected float reloadTimer = 0f;

    // Input
    protected PlayerInputActions playerControls;
    protected InputAction primaryFire;
    protected InputAction secondaryFire;
    protected InputAction reload;

    #endregion

    #region Unity callback functions

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Enable player input systems
    void OnEnable()
    {
        primaryFire = playerControls.Player.PrimaryFire;
        secondaryFire = playerControls.Player.SecondaryFire;
        reload = playerControls.Player.Reload;

        primaryFire.Enable();
        secondaryFire.Enable();
        reload.Enable();

        primaryFire.started += onPrimaryFireStarted;
        secondaryFire.started += onSecondaryFireStarted;
        reload.started += onReload;
    }

    // Disable player input systems
    void OnDisable()
    {
        primaryFire.Disable();
        secondaryFire.Disable();
        reload.Disable();
    }

    void Start()
    {
        currentAmmo = data.magazineCapacity;
        primaryTimer = data.timeBetweenPrimaryFire;
        secondaryTimer = data.timeBetweenSecondaryFire;

        // Set weapon info UI values
        UIManager.getInstance().updateMagazine(currentAmmo, data.magazineCapacity);
        UIManager.getInstance().updateTotalAmmo(WeaponManager.getInstance().totalAmmo);
    }

    void Update()
    {
        primaryTimer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;

        // Set weapon info UI values
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            UIManager.getInstance().updateReloadSlider(reloadTimer / data.timeToReload);
        }
    }

    #endregion

    #region InputAction callback functions

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to shoot their primary
    private void onPrimaryFireStarted(InputAction.CallbackContext context)
    {
        if (inputFrozen) return;

        onPrimaryFire();
    }

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to shoot their secondary
    private void onSecondaryFireStarted(InputAction.CallbackContext context)
    {
        if (inputFrozen) return;

        onSecondaryFire();
    }

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        if (isReloading || currentAmmo >= data.magazineCapacity || WeaponManager.getInstance().totalAmmo <= 0) return;
        if (inputFrozen) return;

        StartCoroutine(StartReload());
    }

    #endregion

    #region Custom functions

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire
    protected abstract void onPrimaryFire();

    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire
    protected abstract void onSecondaryFire();

    // MODIFIES: self
    // EFFECTS: reloads weapon magazine
    private IEnumerator StartReload()
    {
        reloadTimer = 0;
        UIManager.getInstance().updateReloadSlider(0);
        isReloading = true;
        yield return new WaitForSeconds(data.timeToReload);
        reloadTimer = 0;
        UIManager.getInstance().updateReloadSlider(0);
        isReloading = false;
        if (data.magazineCapacity - currentAmmo <= WeaponManager.getInstance().totalAmmo)
        {
            int ammoToAdd = data.magazineCapacity - currentAmmo;
            currentAmmo += ammoToAdd;
            WeaponManager.getInstance().totalAmmo -= ammoToAdd;
        }
        else
        {
            currentAmmo += WeaponManager.getInstance().totalAmmo;
            WeaponManager.getInstance().totalAmmo = 0;
        }
        UIManager.getInstance().updateMagazine(currentAmmo, data.magazineCapacity);
        UIManager.getInstance().updateTotalAmmo(WeaponManager.getInstance().totalAmmo);
    }

    // MODIFIES: self
    // EFFECTS: cancels reload
    public void cancelReload()
    {
        StopCoroutine(StartReload());
        isReloading = false;
    }

    #endregion
}
