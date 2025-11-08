using UnityEngine;

public class CorrectAspectRatio : MonoBehaviour
{
    [Header("Settings")]
    public float targetAspect = 16f / 9f; // 16:9
    public Camera gameCamera;
    public Camera backgroundCamera;

    void Start()
    {
        if (gameCamera == null) gameCamera = Camera.main;
        SetupBackgroundCamera();
        UpdateViewports();
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateViewports();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private int lastWidth, lastHeight;

    void SetupBackgroundCamera()
    {
        // Создаем камеру для фона
        GameObject bgCamObj = new GameObject("BackgroundCamera");
        backgroundCamera = bgCamObj.AddComponent<Camera>();
        backgroundCamera.depth = -10;
        backgroundCamera.clearFlags = CameraClearFlags.SolidColor;
        backgroundCamera.backgroundColor = new Color(0.035f, 0.027f, 0.188f, 1f);
        backgroundCamera.cullingMask = 1 << 9;
    }

    void UpdateViewports()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        
        if (screenAspect > targetAspect)
        {
            // Шире чем 16:9 - черные полосы по бокам
            SetupWiderThanTarget(screenAspect);
        }
        else
        {
            // Уже чем 16:9 (4:3) - черные полосы сверху и снизу
            SetupNarrowerThanTarget(screenAspect);
        }
    }

    void SetupNarrowerThanTarget(float screenAspect)
    {

        float contentHeight = screenAspect / targetAspect;
        float blackBarsHeight = (1f - contentHeight) / 2f;

        gameCamera.rect = new Rect(0f, blackBarsHeight, 1f, contentHeight);

        backgroundCamera.rect = new Rect(0f, 0f, 1f, 1f);

    }

    void SetupWiderThanTarget(float screenAspect)
    {
        float contentWidth = targetAspect / screenAspect;
        float blackBarsWidth = (1f - contentWidth) / 2f;

        gameCamera.rect = new Rect(blackBarsWidth, 0f, contentWidth, 1f);
        backgroundCamera.rect = new Rect(0f, 0f, 1f, 1f);
    }
}