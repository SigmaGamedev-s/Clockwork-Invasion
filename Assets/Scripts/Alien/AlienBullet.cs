using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 10;
    public float speed = 0.8f;
    public float range = 5f;
    public LayerMask turretMask;

    private Vector3 startPos;
    private BasicShooter basicShooter;
    private TurretTile targetTurretTile;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) > range)
        {
            Destroy(gameObject);
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.1f, turretMask);
        Debug.DrawRay(transform.position, Vector2.right * 0.1f, Color.red);

        if (hit.collider)
        {
            basicShooter = hit.collider.GetComponent<BasicShooter>();
            if (basicShooter)
            {
                targetTurretTile = basicShooter.GetComponentInParent<TurretTile>();
                if (targetTurretTile != null)
                {
                    targetTurretTile.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
