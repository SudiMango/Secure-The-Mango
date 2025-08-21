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

    // REQUIRES: data to be of type int
    // MODIFIES: WeaponManager
    // EFFECTS: adds additional ammo to totalAmmo
    public void onAmmoRefill(Component sender, object data)
    {
        PickableItem item = sender as PickableItem;
        if (item.getItemType() != PickableItem.ItemType.AmmoRefill) return;

        int ammoToAdd = (int)data;
        setTotalAmmo(ammoToAdd);
    }
}
