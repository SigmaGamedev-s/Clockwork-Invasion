using UnityEngine;

public class TurretTile : MonoBehaviour
{
    [Header("Tile State")]
    public bool hasBipod;
    public bool hasTurret;

    [Header("Tile Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Audio Settings")]
    public AudioClip hitSound;
    public float volume = 1f;
    private AudioSource audioSource;

    [HideInInspector] public GameObject bipodObject;
    [HideInInspector] public GameObject turretObject;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {

        if (hitSound != null)
        {
            audioSource.pitch = Random.Range(0.85f, 0.95f);
            audioSource.PlayOneShot(hitSound, volume * Random.Range(0.8f, 1f));
        }
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyTurretAndBipod();
            ResetTileState();
        }
    }

    private void DestroyTurretAndBipod()
    {
        if (turretObject != null)
            Destroy(turretObject);

        if (bipodObject != null)
            Destroy(bipodObject);
    }

    private void ResetTileState()
    {
        hasTurret = false;
        hasBipod = false;
        turretObject = null;
        bipodObject = null;
        currentHealth = maxHealth;
    }
}

