using UnityEngine;
using System.Collections;

public class Alien : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 1f;
    public int health = 100;
    public float range = 1.5f;
    public int damage = 10;

    [Header("Attack")]
    public LayerMask turretMask;
    public float cooldown = 1.5f;
    protected bool canShoot = true;

    [HideInInspector] public BasicShooter basicShooter;
    protected TurretTile targetTurretTile;

    protected AlienAnimatorController anim;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<AlienAnimatorController>();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, turretMask);
        Debug.DrawRay(transform.position, Vector2.right * range, Color.red);

        if (hit.collider)
        {
            basicShooter = hit.collider.GetComponent<BasicShooter>();
            if (basicShooter)
            {
                targetTurretTile = basicShooter.GetComponentInParent<TurretTile>();
                anim.SetWalking(false);
                Shoot();
                return;
            }
        }

        basicShooter = null;
        targetTurretTile = null;
        anim.SetWalking(true);
    }

    protected virtual void FixedUpdate()
    {
        if (isDead) return;

        if (!basicShooter)
        {
            transform.position += Vector3.right * speed * Time.fixedDeltaTime;
        }
    }

    protected virtual void Shoot()
    {
        if (!canShoot || targetTurretTile == null)
            return;

        canShoot = false;
        anim.PlayAttack();
        targetTurretTile.TakeDamage(damage);

        Invoke(nameof(ResetCooldown), cooldown);
    }

    protected void ResetCooldown()
    {
        canShoot = true;
    }

    public virtual void Hit(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        anim.PlayDeath();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f);
    }
}
