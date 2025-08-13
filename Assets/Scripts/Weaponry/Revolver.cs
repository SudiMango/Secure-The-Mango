using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    private float damageMultiplier = 0.7f;

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire, shoots 1 bullet out of gun
    public override void onPrimaryFire()
    {
        if (!canShoot) return;

        if (primaryTimer >= data.timeBetweenPrimaryFire && currentAmmo > 0 && !isReloading)
        {
            primaryTimer = 0;

            GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                                    firePoint.position,
                                                    firePoint.rotation,
                                                    WeaponManager.getInstance().bulletParent);
            Bullet bh = t_bullet.GetComponent<Bullet>();
            bh.setDamage(data.damage);
            bh.setSpeed(data.bulletSpeed);
            bh.setDir(getDir());
            bh.setEnemyTag(owner.tag);
            bh.startBullet();

            currentAmmo -= 1;

            onShoot.raise(owner, new GunFiredEventData(currentAmmo, data.magazineCapacity, new Bullet[] { bh }, firePoint, getDir()));
        }
    }


    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire, does "fan the hammer" effect on all bullets left in magazine
    public override void onSecondaryFire()
    {
        if (!canShoot) return;

        if (secondaryTimer >= data.timeBetweenSecondaryFire && currentAmmo > 0 && !isReloading)
        {
            secondaryTimer = 0;
            StartCoroutine(FanTheHammer());
        }
    }

    // MODIFIES: self, bullet
    // EFFECTS: perform "fan the hammer" effect on all bullets left in magazine, damage is reduced
    private IEnumerator FanTheHammer()
    {
        canShoot = false;

        int numRounds = currentAmmo;
        for (int i = 0; i < numRounds; i++)
        {
            GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                            firePoint.position,
                                            firePoint.rotation,
                                            WeaponManager.getInstance().bulletParent);
            Bullet bh = t_bullet.GetComponent<Bullet>();
            bh.setDamage((float)(data.damage * damageMultiplier));
            bh.setSpeed(data.bulletSpeed);
            bh.setDir(getDir());
            bh.setEnemyTag(owner.tag);
            bh.startBullet();

            currentAmmo -= 1;

            onShoot.raise(owner, new GunFiredEventData(currentAmmo, data.magazineCapacity, new Bullet[] { bh }, firePoint, getDir()));

            yield return new WaitForSeconds(0.25f);
        }

        canShoot = true;
    }
}
