using UnityEngine;

public enum ResourceType
{
    Energy,
    Gear
}

public class ResourcePickup : Sounds
{
    public ResourceType resourceType = ResourceType.Energy;
    public float lifetime = 10f;
    public int amount;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Более надежный метод для кликов
    private void OnMouseDown()
    {
        Collect();
    }

    // Альтернатива: проверка через Raycast
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        if (ScoreCounter.Instance == null) return;

        if (resourceType == ResourceType.Energy)
            ScoreCounter.Instance.AddToEnergy(amount);
        else
            ScoreCounter.Instance.AddToGear(amount);

        PlaySound(0, destroyed: true);
        Destroy(gameObject);
    }
}