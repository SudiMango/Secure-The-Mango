using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Shotgun : Gun
{

    private float primaryRot = -14;
    private float secondaryRot = -7;

    private float damageMultiplier = 1.5f;
    private float speedMultiplier = 1.5f;

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire, shoots 5 shotgun pellets out of gun
    public override void onPrimaryFire()
    {
        if (!canShoot) return;

        if (primaryTimer >= data.timeBetweenPrimaryFire && currentAmmo > 0 && !isReloading)
        {
            primaryTimer = 0;

            float rot = primaryRot;
            Bullet[] bullets = new Bullet[5];
            for (int i = 0; i < 5; i++)
            {
                GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                                firePoint.position,
                                                firePoint.rotation * Quaternion.Euler(0, 0, rot),
                                                WeaponManager.getInstance().bulletParent);
                Bullet bh = t_bullet.GetComponent<Bullet>();
                bh.setDamage(data.damage);
                bh.setSpeed(data.bulletSpeed);
                bh.setDir(getDir());
                bh.setEnemyTag(owner.tag);
                bh.startBullet();
                rot += math.abs(primaryRot) / 2;
                bullets[i] = bh;
            }

            currentAmmo -= 1;
            onShoot.raise(owner, new GunFiredEventData(currentAmmo, data.magazineCapacity, bullets, firePoint, getDir()));
        }
    }


    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire, does a faster three-shot higher damage pellet and 
    //          propels player towards opposite direction of fire
    public override void onSecondaryFire()
    {
        if (!canShoot) return;

        if (secondaryTimer >= data.timeBetweenSecondaryFire && currentAmmo > 0 && !isReloading)
        {
            secondaryTimer = 0;
            float rot = secondaryRot;
            Bullet[] bullets = new Bullet[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                                firePoint.position,
                                                firePoint.rotation * Quaternion.Euler(0, 0, rot),
                                                WeaponManager.getInstance().bulletParent);
                Bullet bh = t_bullet.GetComponent<Bullet>();
                bh.setDamage((float)(data.damage * damageMultiplier));
                bh.setSpeed((float)(data.bulletSpeed * speedMultiplier));
                bh.setDir(getDir());
                bh.setEnemyTag(owner.tag);
                bh.startBullet();
                rot += math.abs(secondaryRot);
                bullets[i] = bh;
            }

            currentAmmo -= 1;
            onShoot.raise(owner, new GunFiredEventData(currentAmmo, data.magazineCapacity, bullets, firePoint, getDir()));
            StartCoroutine(PropelPlayer());
        }
    }

    // MODIFIES: player
    // EFFECTS: propel player towards opposite direction of fire
    private IEnumerator PropelPlayer()
    {
        GameObject plr = GameObject.FindWithTag("Player");
        PlayerMovementController m = plr.GetComponent<PlayerMovementController>();
        Rigidbody2D rb = plr.GetComponent<Rigidbody2D>();

        Vector2 dir = firePoint.right * -getDir();

        m.disableMovement();
        rb.linearVelocity = dir * 15;
        yield return new WaitForSeconds(0.5f);
        m.enableMovement();
    }
}
