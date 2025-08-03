using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private WeaponDataScriptableObject data;
    [SerializeField] private Transform firePoint;

    // Other required variables
    private int currentAmmo;
    private bool isReloading = false;
    private float shootTimer = 0f;
    private float reloadTimer = 0f;

    private Transform bulletParent;

    private Transform magazineText;
    private Transform totalAmmoText;
    private Transform reloadSlider;

    // Input
    private PlayerInputActions playerControls;
    private InputAction fire;
    private InputAction reload;

    #endregion

    #region Unity callback functions

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Enable player input systems
    void OnEnable()
    {
        fire = playerControls.Player.Attack;
        reload = playerControls.Player.Reload;

        fire.Enable();
        reload.Enable();

        fire.started += onFireStarted;
        reload.started += onReload;
    }

    // Disable player input systems
    void OnDisable()
    {
        fire.Disable();
        reload.Disable();
    }

    void Start()
    {
        currentAmmo = data.magazineCapacity;
        shootTimer = data.timeBetweenFire;

        bulletParent = GameObject.Find("FX").transform;

        // Weapon info UI
        GameObject ui = Instantiate(WeaponManager.Instance.weaponInfoGuiPrefab, GameObject.Find("Canvas").transform);

        magazineText = ui.transform.Find("Magazine");
        totalAmmoText = ui.transform.Find("TotalAmmo");
        reloadSlider = ui.transform.Find("ReloadSlider");
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Set weapon info UI values
        magazineText.GetComponent<Text>().text = currentAmmo + "/" + data.magazineCapacity;
        totalAmmoText.GetComponent<Text>().text = WeaponManager.Instance.totalAmmo.ToString();

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
    private void onFireStarted(InputAction.CallbackContext context)
    {
        if (shootTimer >= data.timeBetweenFire && currentAmmo > 0 && !isReloading)
        {
            shootTimer = 0;
            shoot();
        }
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
    // EFFECTS: shoots bullet out of gun
    private void shoot()
    {
        GameObject t_bullet = Instantiate(WeaponManager.Instance.bulletPrefab, firePoint.position, firePoint.rotation, bulletParent);
        BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
        bh.setEnemyTag("Enemy");
        bh.setDamage(data.damage);

        currentAmmo -= 1;
    }

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
