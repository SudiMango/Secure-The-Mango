using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    #region Variables

    [Header("Basic info")]
    [SerializeField] private string gunName;
    [SerializeField] private float timeBetweenFire;
    [SerializeField] private float damage;

    [Header("Instancing info")]
    [SerializeField] private Vector3 pos = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 rot = new Vector3(0, 0, 0);

    [Header("Ammo related")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int magazineCapacity;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float timeToReload;

    [Header("References")]
    [SerializeField] private GameObject weaponInfoGui;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    private Transform bulletParent;

    // Other required variables
    private bool isReloading = false;
    private float shootTimer = 0f;
    private float reloadTimer = 0f;

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
        currentAmmo = magazineCapacity;
        shootTimer = timeBetweenFire;

        bulletParent = GameObject.Find("FX").transform;

        // Weapon info UI
        GameObject ui = Instantiate(weaponInfoGui, GameObject.Find("Canvas").transform);

        magazineText = ui.transform.Find("Magazine");
        totalAmmoText = ui.transform.Find("TotalAmmo");
        reloadSlider = ui.transform.Find("ReloadSlider");
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Set weapon info UI values
        magazineText.GetComponent<Text>().text = currentAmmo + "/" + magazineCapacity;
        totalAmmoText.GetComponent<Text>().text = totalAmmo.ToString();

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            reloadSlider.GetComponent<Slider>().value = reloadTimer / timeToReload;
        }
    }

    #endregion

    #region InputAction callback functions

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to shoot their weapon
    private void onFireStarted(InputAction.CallbackContext context)
    {
        if (shootTimer >= timeBetweenFire && currentAmmo > 0 && !isReloading)
        {
            shootTimer = 0;
            shoot();
        }
    }

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to reload their weapon
    private void onReload(InputAction.CallbackContext context)
    {
        if (isReloading || currentAmmo >= magazineCapacity || totalAmmo <= 0) return;

        StartCoroutine(StartReload());
    }

    #endregion

    #region Custom functions

    // MODIFIES: self, bullet
    // EFFECTS: shoots bullet out of gun
    private void shoot()
    {
        GameObject t_bullet = Instantiate(bullet, firePoint.position, firePoint.rotation, bulletParent);
        BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
        bh.setEnemyTag("Enemy");
        bh.setDamage(damage);

        currentAmmo -= 1;
    }

    // MODIFIES: self
    // EFFECTS: reloads weapon magazine
    private IEnumerator StartReload()
    {
        reloadTimer = 0;
        reloadSlider.GetComponent<Slider>().value = 0;
        isReloading = true;
        yield return new WaitForSeconds(timeToReload);
        reloadTimer = 0;
        reloadSlider.GetComponent<Slider>().value = 0;
        isReloading = false;
        if (magazineCapacity - currentAmmo <= totalAmmo)
        {
            int ammoToAdd = magazineCapacity - currentAmmo;
            currentAmmo += ammoToAdd;
            totalAmmo -= ammoToAdd;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
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
