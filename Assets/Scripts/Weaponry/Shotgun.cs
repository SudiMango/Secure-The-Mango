using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : Gun
{

    private float primaryRot = -14;
    private float secondaryRot = -7;

    private float damageMultiplier = 1.5f;
    private float speedMultiplier = 1.5f;

    // MODIFIES: self, bullet
    // EFFECTS: primary method of fire, shoots 5 shotgun pellets out of gun
    protected override void onPrimaryFire()
    {
        if (primaryTimer >= data.timeBetweenPrimaryFire && currentAmmo > 0 && !isReloading)
        {
            primaryTimer = 0;

            float rot = primaryRot;
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
                rot += math.abs(primaryRot) / 2;
            }

            currentAmmo -= 1;

            UIManager.getInstance().updateMagazine(currentAmmo, data.magazineCapacity);
        }
    }


    // MODIFIES: self, bullet
    // EFFECTS: secondary method of fire, does a faster three-shot higher damage pellet and 
    //          propels player towards opposite direction of fire
    protected override void onSecondaryFire()
    {
        if (secondaryTimer >= data.timeBetweenSecondaryFire && currentAmmo > 0 && !isReloading)
        {
            secondaryTimer = 0;
            float rot = secondaryRot;
            for (int i = 0; i < 3; i++)
            {
                GameObject t_bullet = Instantiate(WeaponManager.getInstance().bulletPrefab,
                                                firePoint.position,
                                                firePoint.rotation * Quaternion.Euler(0, 0, rot),
                                                WeaponManager.getInstance().bulletParent);
                BulletHandler bh = t_bullet.GetComponent<BulletHandler>();
                bh.setEnemyTag("Enemy");
                bh.setDamage((float)(data.damage * damageMultiplier));
                bh.setSpeed((float)(data.bulletSpeed * speedMultiplier));
                rot += math.abs(secondaryRot);
            }

            currentAmmo -= 1;
            UIManager.getInstance().updateMagazine(currentAmmo, data.magazineCapacity);
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

    // EFFECTS: returns the direction based on player's x localscale
    private int getDir()
    {
        if (GameObject.FindWithTag("Player").transform.localScale.x < 0)
        {
            return -1;
        }
        return 1;
    }
}
