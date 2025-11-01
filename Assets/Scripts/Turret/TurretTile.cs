using UnityEngine;

public class TurretTile : MonoBehaviour
{
    [Header("Tile State")]
    public bool hasBipod;
    public bool hasTurret;

    [Header("Tile Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [HideInInspector] public GameObject bipodObject;
    [HideInInspector] public GameObject turretObject;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
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

