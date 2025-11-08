using UnityEngine;
using System.Collections;

public class Alien : Sounds
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

    [Header("Audio Settings")]
    public float volume = 1f;

    [Header("Drop Settings")]
    public ResourceDrop[] possibleDrops;
    public float dropSpread = 0.5f;

    [HideInInspector] public BasicShooter basicShooter;
    protected TurretTile targetTurretTile;
    protected AlienAnimatorController anim;

    protected bool isDead = false;
    protected bool isKnockback = false;
    protected float originalSpeed;
    protected float originalCooldown;
    protected Color originalColor;

    public bool LastAlien;

    protected const float KNOCKBACK_FORCE = 4f;
    protected const float KNOCKBACK_DURATION = 0.3f;

    protected Vector2 knockbackDirection;
    protected float knockbackTimer;

    private float randomVoiceTimer;
    protected bool isFrozen = false;

    protected override void Awake()
    {
        anim = GetComponentInChildren<AlienAnimatorController>();

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        originalSpeed = speed;
        originalCooldown = cooldown;

        // стартуем таймер для случайных звуков
        ScheduleRandomVoice();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
                isKnockback = false;
            return;
        }

        // --- случайные звуки пришельца ---
        randomVoiceTimer -= Time.deltaTime;
        if (randomVoiceTimer <= 0)
        {
            PlaySound(2, 0.2f);
            ScheduleRandomVoice();
        }

        // --- обнаружение цели ---
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, turretMask);

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

        if (isKnockback)
        {
            transform.position += (Vector3)knockbackDirection * KNOCKBACK_FORCE * Time.fixedDeltaTime;
        }
        else if (!basicShooter)
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

    // ====================== УРОН ======================

    public virtual void Hit(int damage, bool isFreeze, bool isKnockBack)
    {
        if (isDead) return;

        PlaySound(1, 1.2f);

        if (isKnockBack)
            ApplyKnockback();

        health -= damage;

        if (isFreeze)
            Freeze();

        if (health <= 0)
        {
            if (LastAlien)
                GameObject.Find("GameManager").GetComponent<Gamemanager>().Win();

            Die();
        }
    }

    public virtual void Hit(int damage, bool isFreeze)
    {
        Hit(damage, isFreeze, false);
    }

    // ====================== ЭФФЕКТЫ ======================

    protected virtual void ApplyKnockback()
    {
        isKnockback = true;
        knockbackTimer = KNOCKBACK_DURATION;
        knockbackDirection = Vector2.left;
        anim.SetWalking(false);
    }

    protected virtual void Freeze()
    {
        if (isFrozen)
        {
            CancelInvoke(nameof(UnFreeze));
            Invoke(nameof(UnFreeze), 5);
            return;
        }

        isFrozen = true;

        CancelInvoke(nameof(UnFreeze));

        speed = originalSpeed * 0.5f;
        cooldown = originalCooldown * 2f;

        PlaySound(0, 1f);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(0.5f, 0.7f, 1f);

        Invoke(nameof(UnFreeze), 5);
    }

    protected virtual void UnFreeze()
    {
        isFrozen = false;

        speed = originalSpeed;
        cooldown = originalCooldown;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    protected virtual void Die()
    {
        isDead = true;
        anim.PlayDeath();
        GetComponent<Collider2D>().enabled = false;

        CancelInvoke(nameof(UnFreeze));
        CancelInvoke(nameof(ResetCooldown));
        TryDropResources();

        Destroy(gameObject, 1.5f);
    }

    protected void TryDropResources()
    {
        if (possibleDrops == null || possibleDrops.Length == 0) return;

        foreach (var drop in possibleDrops)
        {
            if (drop.prefab == null) continue;

            if (Random.value <= drop.chance)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-dropSpread, dropSpread),
                    Random.Range(-dropSpread, dropSpread),
                    0f
                );

                GameObject newDrop = Instantiate(drop.prefab, transform.position + offset, Quaternion.identity);

                var pickup = newDrop.GetComponent<ResourcePickup>();
            }
        }
    }

    private void ScheduleRandomVoice()
    {
        randomVoiceTimer = Random.Range(10f, 25f);
    }
}
