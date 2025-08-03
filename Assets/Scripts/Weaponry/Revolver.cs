using UnityEngine;

public class Revolver : Gun
{

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire, shoots 1 bullet out of gun
    protected override void onPrimaryFire()
    {
        GameObject t_bullet = Instantiate(WeaponManager.Instance.bulletPrefab,
                                            firePoint.position,
                                            firePoint.rotation,
                                            WeaponManager.Instance.bulletParent);
        BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
        bh.setEnemyTag("Enemy");
        bh.setDamage(data.damage);
        bh.setSpeed(data.bulletSpeed);

        currentAmmo -= 1;
    }


    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire, does "fan the hammer" effect on all bullets left in magazine
    protected override void onSecondaryFire()
    {
        Debug.Log("Secondary fire");
    }
}
