using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : MonoBehaviour
{
    #region Variables

    // Public variables
    public WeaponDataScriptableObject Data => data;
    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;
    public float ReloadTimer => reloadTimer;

    // Events
    public event Action onReloadStarted;
    public event Action onReloadEnded;

    [Header("References")]
    [SerializeField] protected WeaponDataScriptableObject data;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform shooter;

    // Timers
    protected float primaryTimer = 0f;
    protected float secondaryTimer = 0f;
    protected float reloadTimer = 0f;

    // Other required variables
    protected int currentAmmo;
    protected bool isReloading = false;
    protected bool canShoot = true;

    #endregion

    #region Unity callback functions

    void Start()
    {
        // Set default values
        currentAmmo = data.magazineCapacity;
        primaryTimer = data.timeBetweenPrimaryFire;
        secondaryTimer = data.timeBetweenSecondaryFire;
    }

    void Update()
    {
        // Update timers
        primaryTimer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
        }
    }

    #endregion

    #region Custom functions

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire
    public abstract void onPrimaryFire();

    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire
    public abstract void onSecondaryFire();

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to reload their weapon
    public void onReload()
    {
        if (isReloading || currentAmmo >= data.magazineCapacity || WeaponManager.getInstance().totalAmmo <= 0) return;
        if (!canShoot) return;

        StartCoroutine(StartReload());
    }

    // MODIFIES: self
    // EFFECTS: cancels reload
    public void cancelReload()
    {
        StopCoroutine(StartReload());
        isReloading = false;
    }

    // MODIFIES: self
    // EFFECTS: reloads weapon magazine
    private IEnumerator StartReload()
    {
        reloadTimer = 0;
        onReloadStarted?.Invoke();
        isReloading = true;
        yield return new WaitForSeconds(data.timeToReload);
        reloadTimer = 0;
        onReloadEnded?.Invoke();
        isReloading = false;
        if (shooter.transform.gameObject.CompareTag("Player"))
        {
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
        }
        else if (shooter.transform.gameObject.CompareTag("Enemy"))
        {
            int ammoToAdd = data.magazineCapacity - currentAmmo;
            currentAmmo += ammoToAdd;
        }
    }

    // EFFECTS: returns the direction depending on x localscale of root entity
    protected int getDir()
    {
        if (shooter.localScale.x > 0)
        {
            return 1;
        }

        return -1;
    }

    #endregion
}
