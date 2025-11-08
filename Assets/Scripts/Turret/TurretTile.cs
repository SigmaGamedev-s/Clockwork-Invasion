using UnityEngine;

public class TurretTile : Sounds
{

    public SpriteRenderer highlightRenderer;

    [Header("Tile State")]
    public bool hasBipod;
    public bool hasTurret;

    [Header("Tile Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Tile Type")]
    public TurretType bipodType = TurretType.None;
    public TurretType turretType = TurretType.None;

    [HideInInspector] public GameObject bipodObject;
    [HideInInspector] public GameObject turretObject;


    protected override void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        PlaySound(0, 1f * Random.Range(0.8f, 1f));
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

    public void RemoveTurret()
    {
        if (hasTurret || hasBipod)
        {
            DestroyTurretAndBipod();
            ResetTileState();
        }
    }

}

