using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    #region Variables

    // Public variables
    public WeaponDataScriptableObject Data => data;
    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;

    // Events
    [Header("Events")]
    [SerializeField] protected GameEvent updateAmmoGui;
    [SerializeField] private GameEvent onReloadStarted;
    [SerializeField] private GameEvent onReloadEnded;

    // Timers
    protected float primaryTimer = 0f;
    protected float secondaryTimer = 0f;

    // Other required variables
    protected int currentAmmo;
    protected bool isReloading = false;
    protected bool canShoot = true;


    #endregion

    #region Unity callback functions

    private void Start()
    {
        // Set default values
        primaryTimer = data.timeBetweenPrimaryFire;
        secondaryTimer = data.timeBetweenSecondaryFire;
        currentAmmo = data.magazineCapacity;

        updateAmmoGui.raise(owner, new object[] { currentAmmo, data.magazineCapacity });
    }

    private void Update()
    {
        // Update timers
        primaryTimer += Time.deltaTime;
        secondaryTimer += Time.deltaTime;
    }

    #endregion

    #region Custom functions

    // EFFECTS: primary method of fire
    public abstract void onPrimaryFire();

    // EFFECTS: secondary method of fire
    public abstract void onSecondaryFire();

    // MODIFIES: self
    // EFFECTS: callback function for when player tries to reload their weapon
    public void onReload()
    {
        if (isReloading || currentAmmo >= data.magazineCapacity || WeaponManager.getInstance().totalAmmo <= 0) return;
        if (!owner) return;

        StartCoroutine(StartReload());
    }

    // MODIFIES: self
    // EFFECTS: cancels reload
    public void cancelReload()
    {
        StopCoroutine(StartReload());
        isReloading = false;

        onReloadEnded.raise(owner, null);
    }

    // MODIFIES: self
    // EFFECTS: reloads weapon magazine
    private IEnumerator StartReload()
    {
        isReloading = true;
        onReloadStarted.raise(owner, data.timeToReload);

        yield return new WaitForSeconds(data.timeToReload);

        isReloading = false;
        onReloadEnded.raise(owner, null);

        if (owner.transform.gameObject.CompareTag("Player"))
        {
            if (data.magazineCapacity - currentAmmo <= WeaponManager.getInstance().totalAmmo)
            {
                int ammoToAdd = data.magazineCapacity - currentAmmo;
                currentAmmo += ammoToAdd;
                WeaponManager.getInstance().setTotalAmmo(-ammoToAdd);
            }
            else
            {
                currentAmmo += WeaponManager.getInstance().totalAmmo;
                WeaponManager.getInstance().setTotalAmmo(-WeaponManager.getInstance().totalAmmo);
            }
            updateAmmoGui.raise(owner, new object[] { currentAmmo, data.magazineCapacity });
        }
        else if (owner.transform.gameObject.CompareTag("Enemy"))
        {
            int ammoToAdd = data.magazineCapacity - currentAmmo;
            currentAmmo += ammoToAdd;
        }

    }

    // EFFECTS: returns the direction depending on x localscale of root entity
    protected int getDir()
    {
        if (owner.localScale.x > 0)
        {
            return 1;
        }

        return -1;
    }

    #endregion
}
