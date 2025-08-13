using System.Collections;
using UnityEngine;

public abstract class Gun : Weapon
{
    #region Variables

    // Public variables
    public int CurrentAmmo => currentAmmo;
    public bool IsReloading => isReloading;

    // Events
    [Header("Events")]
    [SerializeField] protected GameEvent updateAmmoGui;
    [SerializeField] private GameEvent onReloadStarted;
    [SerializeField] private GameEvent onReloadEnded;

    // Other required variables
    protected int currentAmmo;
    protected bool isReloading = false;

    #endregion

    #region Unity callback functions

    protected override void Start()
    {
        base.Start();

        // Set default values
        currentAmmo = data.magazineCapacity;

        updateAmmoGui.raise(owner, new object[] { currentAmmo, data.magazineCapacity });
    }

    #endregion

    #region Custom functions

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

    #endregion
}
