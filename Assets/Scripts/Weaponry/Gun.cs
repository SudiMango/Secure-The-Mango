using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Gun : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] protected WeaponDataScriptableObject data;
    [SerializeField] protected Transform firePoint;

    // Other required variables
    protected int currentAmmo;
    protected bool isReloading = false;
    protected float shootTimer = 0f;
    protected float reloadTimer = 0f;


    // UI
    protected Transform magazineText;
    protected Transform totalAmmoText;
    protected Transform reloadSlider;

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
        shootTimer = data.timeBetweenFire;

        // Weapon info UI
        GameObject ui = Instantiate(WeaponManager.Instance.weaponInfoGuiPrefab, GameObject.Find("MainCanvas").transform);

        magazineText = ui.transform.Find("Magazine");
        totalAmmoText = ui.transform.Find("TotalAmmo");
        reloadSlider = ui.transform.Find("ReloadSlider");

        // Set weapon info UI values
        magazineText.GetComponent<Text>().text = currentAmmo + "/" + data.magazineCapacity;
        totalAmmoText.GetComponent<Text>().text = WeaponManager.Instance.totalAmmo.ToString();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Set weapon info UI values
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            reloadSlider.GetComponent<Slider>().value = reloadTimer / data.timeToReload;
        }
    }

    #endregion

    #region InputAction callback functions

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to shoot their weapon
    private void onPrimaryFireStarted(InputAction.CallbackContext context)
    {
        if (shootTimer >= data.timeBetweenFire && currentAmmo > 0 && !isReloading)
        {
            shootTimer = 0;
            onPrimaryFire();
            magazineText.GetComponent<Text>().text = currentAmmo + "/" + data.magazineCapacity;
        }
    }

    private void onSecondaryFireStarted(InputAction.CallbackContext context)
    {
        onSecondaryFire();
    }

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        if (isReloading || currentAmmo >= data.magazineCapacity || WeaponManager.Instance.totalAmmo <= 0) return;

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
        reloadSlider.GetComponent<Slider>().value = 0;
        isReloading = true;
        yield return new WaitForSeconds(data.timeToReload);
        reloadTimer = 0;
        reloadSlider.GetComponent<Slider>().value = 0;
        isReloading = false;
        if (data.magazineCapacity - currentAmmo <= WeaponManager.Instance.totalAmmo)
        {
            int ammoToAdd = data.magazineCapacity - currentAmmo;
            currentAmmo += ammoToAdd;
            WeaponManager.Instance.totalAmmo -= ammoToAdd;
        }
        else
        {
            currentAmmo += WeaponManager.Instance.totalAmmo;
            WeaponManager.Instance.totalAmmo = 0;
        }
        magazineText.GetComponent<Text>().text = currentAmmo + "/" + data.magazineCapacity;
        totalAmmoText.GetComponent<Text>().text = WeaponManager.Instance.totalAmmo.ToString();
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
