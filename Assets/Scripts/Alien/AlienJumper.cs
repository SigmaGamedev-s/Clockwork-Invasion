using UnityEngine;
using System.Collections;

public class AlienJumper : Alien
{
    [Header("Jump Settings")]
    public float jumpForce = 1f;
    public float jumpHeight = 0.5f;
    public float jumpInterval = 1f;

    [Header("Detection Settings")]
    public float detectDistance = 3f;
    public float stopOffset = 0.1f;

    private bool isJumping = false;

    private float originalJumpForce;
    private float originalJumpHeight;
    private float originalJumpInterval;

    protected override void Awake()
    {
        base.Awake();
        originalJumpForce = jumpForce;
        originalJumpHeight = jumpHeight;
        originalJumpInterval = jumpInterval;
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update(); // Вызываем базовый Update для проверки атаки
    }

    protected override void FixedUpdate()
    {
        if (isDead) return;

        // Применяем откидывание если оно активно
        if (isKnockback)
        {
            transform.position += (Vector3)knockbackDirection * KNOCKBACK_FORCE * Time.fixedDeltaTime;
        }

        // Прыжки работают независимо от откидывания
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectDistance, turretMask);

        if (hit.collider && !isJumping)
        {
            float turretLeftEdge = hit.collider.bounds.min.x;
            float targetX = turretLeftEdge - stopOffset;

            if (targetX > transform.position.x + 0.1f)
            {
                StartCoroutine(JumpToPosition(targetX));
            }
        }
        else if (!isJumping)
        {
            StartCoroutine(JumpForward());
        }
    }

    protected override void ApplyKnockback()
    {
        base.ApplyKnockback(); // Вызываем базовый метод откидывания
        // Не останавливаем прыжки - просто добавляем откидывание
    }

    private IEnumerator JumpForward()
    {
        isJumping = true;

        anim.SetIdle(true);
        anim.PlayJump();
        yield return new WaitForSeconds(0.1f);

        float jumpTime = 0.25f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + Vector3.right * jumpForce;

        while (elapsed < jumpTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // Базовое движение прыжка
            Vector3 jumpMovement = Vector3.Lerp(startPos, targetPos, t) + Vector3.up * height;

            // Если есть откидывание - добавляем его к позиции
            if (isKnockback)
            {
                jumpMovement += (Vector3)knockbackDirection * KNOCKBACK_FORCE * Time.deltaTime;
            }

            transform.position = jumpMovement;
            yield return null;
        }

        // Финализируем позицию только если нет откидывания
        if (!isKnockback)
        {
            transform.position = targetPos;
        }

        anim.SetIdle(true);

        yield return new WaitForSeconds(jumpInterval);
        isJumping = false;
    }

    private IEnumerator JumpToPosition(float targetX)
    {
        isJumping = true;

        anim.SetIdle(true);
        anim.PlayJump();
        yield return new WaitForSeconds(0.1f);

        float jumpTime = 0.25f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);

        while (elapsed < jumpTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpTime;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // Базовое движение прыжка
            Vector3 jumpMovement = Vector3.Lerp(startPos, targetPos, t) + Vector3.up * height;

            // Если есть откидывание - добавляем его к позиции
            if (isKnockback)
            {
                jumpMovement += (Vector3)knockbackDirection * KNOCKBACK_FORCE * Time.deltaTime;
            }

            transform.position = jumpMovement;
            yield return null;
        }

        // Финализируем позицию только если нет откидывания
        if (!isKnockback)
        {
            transform.position = targetPos;
        }

        anim.SetIdle(true);

        yield return new WaitForSeconds(jumpInterval);
        isJumping = false;
    }

    protected override void Freeze()
    {
        CancelInvoke(nameof(UnFreeze));

        jumpForce = originalJumpForce * 0.5f;
        jumpInterval = originalJumpInterval * 2f;
        cooldown = originalCooldown * 2f;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 0.7f, 1f);
        }

        Invoke(nameof(UnFreeze), 5f);
    }

    protected override void UnFreeze()
    {
        jumpForce = originalJumpForce;
        jumpInterval = originalJumpInterval;
        cooldown = originalCooldown;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }
}