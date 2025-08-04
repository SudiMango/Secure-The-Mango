using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int totalAmmo;
}
