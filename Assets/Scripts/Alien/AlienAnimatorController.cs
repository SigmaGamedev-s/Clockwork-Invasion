using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AlienAnimatorController : MonoBehaviour
{
    private Animator animator;
    private AlienShooter shooter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        shooter = GetComponentInParent<AlienShooter>();
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsIdle", !isWalking);
    }

    public void SetIdle(bool isIdle)
    {
        animator.SetBool("IsIdle", isIdle);
        animator.SetBool("IsWalking", !isIdle);
    }

    public void PlayJump()
    {
        animator.SetTrigger("Jump");
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }

    public void ResetAll()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Death");
        animator.ResetTrigger("Jump");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsIdle", true);
    }

    public void FireProjectileEvent()
    {
        if (shooter != null)
        {
            shooter.FireProjectile();
        }
    }
}
