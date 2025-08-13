using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int totalAmmo;

    [Header("Events")]
    [SerializeField] private GameEvent onTotalAmmoChanged;

    void Start()
    {
        onTotalAmmoChanged.raise(null, totalAmmo);
    }

    public void setTotalAmmo(int ammoToAdd)
    {
        totalAmmo += ammoToAdd;

        onTotalAmmoChanged.raise(null, totalAmmo);
    }
}
