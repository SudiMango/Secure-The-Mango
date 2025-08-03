using UnityEngine;

[CreateAssetMenu(fileName = "WeaponName", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponDataScriptableObject : ScriptableObject
{
    [Header("Basic info")]
    public string weaponName;
    public float timeBetweenFire;
    public float damage;
    public float bulletSpeed;

    [Header("Ammo related")]
    [SerializeField] public int magazineCapacity;
    [SerializeField] public float timeToReload;

    [Header("Instancing info")]
    public Vector3 pos = new Vector3(0, 0, 0);
    public Vector3 rot = new Vector3(0, 0, 0);
}
