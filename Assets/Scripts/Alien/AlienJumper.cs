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

    protected override void FixedUpdate()
    {
        if (isDead) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectDistance, turretMask);
        Debug.DrawRay(transform.position, Vector2.right * detectDistance, Color.cyan);

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
            transform.position = Vector3.Lerp(startPos, targetPos, t) + Vector3.up * height;
            yield return null;
        }

        transform.position = targetPos;
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
            transform.position = Vector3.Lerp(startPos, targetPos, t) + Vector3.up * height;
            yield return null;
        }

        transform.position = targetPos;
        anim.SetIdle(true);

        yield return new WaitForSeconds(jumpInterval);
        isJumping = false;
    }
}
