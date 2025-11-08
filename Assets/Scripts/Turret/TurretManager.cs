using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance;

    private bool isDeleteMode = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 🔹 Переключение режима удаления
    public void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
    }

    public bool IsDeleteModeActive()
    {
        return isDeleteMode;
    }

    public void EnableDeleteMode()
    {
        if (!isDeleteMode)
        {
            isDeleteMode = true;
        }
    }

    public void DisableDeleteMode()
    {
        if (isDeleteMode)
        {
            isDeleteMode = false;
        }
    }
}
