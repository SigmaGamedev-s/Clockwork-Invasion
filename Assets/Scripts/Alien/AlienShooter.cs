using UnityEngine;
using System.Collections;

public class AlienShooter : Alien
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    protected override void Shoot()
    {
        if (!canShoot || bulletPrefab == null)
            return;

        anim.PlayAttack();

        canShoot = false;
    }

    public void PlayShootSound()
    {
        PlaySound(3);
    }
    public void FireProjectile()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Invoke(nameof(ResetCooldown), cooldown);
    }
}
