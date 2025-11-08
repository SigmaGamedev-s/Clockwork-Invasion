using UnityEngine;

public class BasicShooter : Sounds
{
    [Header("Shooting Settings")]
    public GameObject bullet;
    public Transform shootOrigin;
    public float cooldown = 1f;
    private bool canShoot;

    [Header("Detection Settings")]
    public float range = 5f;
    public LayerMask ShootMask;

    [Header("Audio Settings")]
    public float volume = 0.1f;

    private GameObject Target;

    private void Start()
    {
        gameObject.layer = 8;
        Invoke(nameof(ResetCooldown), 0.5f);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, ShootMask);
        if (hit.collider)
        {
            Target = hit.collider.gameObject;
            Shoot();
        }
    }

    void ResetCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if (!canShoot) return;

        canShoot = false;
        Invoke(nameof(ResetCooldown), cooldown);

        Instantiate(bullet, shootOrigin.position, Quaternion.identity);

        PlaySound(i: 0, volume: volume, p1: 0.85f, p2: 0.95f);
    }
}
