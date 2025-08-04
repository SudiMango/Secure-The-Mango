using UnityEngine;

public class Shotgun : Gun
{

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire, shoots 5 shotgun pellets out of gun
    protected override void onPrimaryFire()
    {
        float rot = -7;
        for (int i = 0; i < 5; i++)
        {
            GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                            firePoint.position,
                                            firePoint.rotation * Quaternion.Euler(0, 0, rot),
                                            WeaponManager.getInstance().bulletParent);
            BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
            bh.setEnemyTag("Enemy");
            bh.setDamage(data.damage);
            bh.setSpeed(data.bulletSpeed);
            rot += 7;
        }

        currentAmmo -= 1;
    }


    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire, does a faster one-shot pellet and propels player 
    //          towards opposite direction of fire
    protected override void onSecondaryFire()
    {
        Debug.Log("Secondary fire");
    }
}
