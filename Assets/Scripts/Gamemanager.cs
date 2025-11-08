using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public GameObject currentTurret;
    public GameObject currentBipod;

    public Sprite currentTurretSprite;
    public Sprite currentBipodSprite;

    public TurretType currentTurretType = TurretType.None;
    public TurretType currentBipodType = TurretType.None;

    public Transform turretTiles;
    public LayerMask turretTileMask;

    private bool isDeleteMode = false;

    private readonly Color greenColor = new Color(0.2f, 0.9f, 0.5f, 0.5f);
    private readonly Color redColor = new Color(1f, 0.3f, 0.4f, 0.5f);
    private readonly Color deleteColor = new Color(1f, 0.2f, 0.6f, 0.5f);

    [HideInInspector] public int currentTurretPrice = 0;
    [HideInInspector] public int currentBipodPrice = 0;

    private WinManager winManager;

    private void Awake()
    {
        winManager = GetComponent<WinManager>();
    }

    private void Update()
    {
        Vector3 fixedMousePos = GetMouseWorldPosition2D(GameObject.FindWithTag("MainCamera").GetComponent<Camera>());
        RaycastHit2D hit = Physics2D.Raycast(
            fixedMousePos,
            Vector2.zero,
            Mathf.Infinity,
            turretTileMask
        );



        // 🔹 Сначала выключаем все подсветки
        foreach (Transform tile in turretTiles)
        {
            var turretTile = tile.GetComponent<TurretTile>();
            if (turretTile && turretTile.highlightRenderer)
                turretTile.highlightRenderer.enabled = false;
        }

        // 🔹 Если выбран режим удаления — подсвечиваем по курсору
        if (isDeleteMode)
        {
            HandleDeleteMode(hit);
            return;
        }

        // 🔹 Если выбрана турель — подсветим все доступные клетки
        if (currentTurret)
            HighlightAvailableTilesForTurret();
        // 🔹 Если выбрана сошка — подсветим все свободные клетки
        else if (currentBipod)
            HighlightAvailableTilesForBipod();

        // 🔹 Подсветка под курсором (чтобы видеть, куда ставишь)
        if (hit.collider)
            HandleTilePlacement(hit.collider);
    }

    public void Win()
    {
        winManager.Win();
    }

    // 🔹 Подсветка турелей
    private void HighlightAvailableTilesForTurret()
    {
        foreach (Transform t in turretTiles)
        {
            var tile = t.GetComponent<TurretTile>();
            if (tile == null || tile.highlightRenderer == null) continue;

            var hr = tile.highlightRenderer;
            hr.enabled = true;

            bool canPlace = tile.hasBipod && !tile.hasTurret && tile.bipodType == currentTurretType;
            hr.color = canPlace ? greenColor : redColor;
        }
    }

    // 🔹 Подсветка сошек
    private void HighlightAvailableTilesForBipod()
    {
        foreach (Transform t in turretTiles)
        {
            var tile = t.GetComponent<TurretTile>();
            if (tile == null || tile.highlightRenderer == null) continue;

            var hr = tile.highlightRenderer;
            hr.enabled = true;
            hr.color = tile.hasBipod ? redColor : greenColor;
        }
    }

    // 🔹 Обработка режима удаления
    private void HandleDeleteMode(RaycastHit2D hit)
    {
        if (!hit.collider) return;
        var tile = hit.collider.GetComponent<TurretTile>();
        if (tile == null || tile.highlightRenderer == null) return;

        var hr = tile.highlightRenderer;
        hr.enabled = true;
        hr.color = (tile.hasTurret || tile.hasBipod) ? deleteColor : redColor;

        if (Input.GetMouseButtonDown(0))
        {
            if (tile.hasTurret || tile.hasBipod)
                tile.TakeDamage(9999);

            // После удаления выключаем режим
            isDeleteMode = false;
        }
    }

    // 🔹 Обработка установки турелей/сошек
    private void HandleTilePlacement(Collider2D col)
    {
        var tile = col.GetComponent<TurretTile>();
        if (tile == null || tile.highlightRenderer == null) return;

        // 🔹 Установка сошек
        if (currentBipod)
        {
            if (Input.GetMouseButtonDown(0) && !tile.hasBipod)
            {
                GameObject newBipod = Instantiate(currentBipod, tile.transform.position, Quaternion.identity);
                newBipod.transform.SetParent(tile.transform, worldPositionStays: true);

                tile.hasBipod = true;
                tile.bipodObject = newBipod;
                tile.bipodType = currentBipodType;

                currentBipod = null;
                currentBipodSprite = null;
                currentBipodPrice = 0;

                TurretOffset bipodScript = newBipod.GetComponent<TurretOffset>();
                if (bipodScript != null)
                    bipodScript.ApplyPlacementOffset();
            }
        }
        // 🔹 Установка турелей
        else if (currentTurret)
        {
            bool canPlace = tile.hasBipod && !tile.hasTurret && tile.bipodType == currentTurretType;

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                GameObject newTurret = Instantiate(currentTurret, tile.transform.position, Quaternion.identity);
                newTurret.transform.SetParent(tile.transform, worldPositionStays: true);

                tile.hasTurret = true;
                tile.turretObject = newTurret;
                tile.turretType = currentTurretType;

                currentTurret = null;
                currentTurretSprite = null;
                currentTurretPrice = 0;

                TurretOffset turretScript = newTurret.GetComponent<TurretOffset>();
                if (turretScript != null)
                    turretScript.ApplyPlacementOffset();
            }
        }
    }

    // 🔹 Включение/отмена режима удаления
    public void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;

        // Сразу отключаем подсветку для всех клеток, если отменяем режим
        if (!isDeleteMode)
        {
            foreach (Transform tile in turretTiles)
            {
                var turretTile = tile.GetComponent<TurretTile>();
                if (turretTile && turretTile.highlightRenderer)
                    turretTile.highlightRenderer.enabled = false;
            }
        }
    }
    private Vector2 GetMouseWorldPosition2D(Camera cam)
    {
        // Просто используем ViewportToWorldPoint с правильными координатами вьюпорта
        Vector3 mousePos = Input.mousePosition;
        Vector3 viewportPos = cam.ScreenToViewportPoint(mousePos);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        return worldPos;
    }

}

