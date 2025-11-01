using UnityEngine;
using System.Collections;

public class AlienShooter : Alien
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    public AudioClip shootSound;

    protected override void Shoot()
    {
        if (!canShoot || bulletPrefab == null)
            return;

        anim.PlayAttack();

        canShoot = false;
    }

    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    public void FireProjectile()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Invoke(nameof(ResetCooldown), cooldown);
    }
}
