using UnityEngine;
using UnityEngine.InputSystem;

public class GunHandler : MonoBehaviour
{
    #region Variables

    [Header("Basic info")]
    [SerializeField] private string gunName;
    [SerializeField] private bool isAuto = false;
    [SerializeField] private float timeBetweenFire;
    [SerializeField] private float damage;

    [Header("Ammo related")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int magazineCapacity;
    [SerializeField] private int totalAmmo;
    [SerializeField] private float timeToReload;

    [Header("References")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform bulletParent;

    // Other required variables
    private float shootTimer = 0f;
    private float reloadTimer = 0f;

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
        fire.canceled += onFireEnded;

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
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
    }

    #endregion

    #region InputAction callback functions

    private void onFireStarted(InputAction.CallbackContext context)
    {
        if (shootTimer >= timeBetweenFire && currentAmmo > 0)
        {
            shootTimer = 0;
            shoot();
        }
    }

    private void onFireEnded(InputAction.CallbackContext context)
    {
        print("Fire ended");
    }

    private void onReload(InputAction.CallbackContext context)
    {
        print("Reloading!");
    }

    #endregion

    #region Custom functions

    private void shoot()
    {
        GameObject t_bullet = Instantiate(bullet, firePoint.position, firePoint.rotation, bulletParent);
        BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
        bh.setEnemyTag("Enemy");
        bh.setDamage(damage);
    }

    private void cancelReload()
    {
        print("Cancelled reload!");
    }

    #endregion
}
