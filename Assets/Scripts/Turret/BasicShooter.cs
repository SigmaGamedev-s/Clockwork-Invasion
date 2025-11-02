using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooter : MonoBehaviour
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
    public AudioClip shootSound;
    public float volume = 0.1f;// <-- звук выстрела
    private AudioSource audioSource;    // источник звука

    private GameObject Target;

    private void Start()
    {
        // создаем или получаем AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;  // чтобы не проигрывался при старте
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

        if (shootSound != null)
        {
            audioSource.pitch = Random.Range(0.85f, 0.95f);
            audioSource.PlayOneShot(shootSound, volume * Random.Range(0.8f, 1f));
        }
    }
}
