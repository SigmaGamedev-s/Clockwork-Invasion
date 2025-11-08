using UnityEngine;
using UnityEditor;

public class AddOutlineToTiles : EditorWindow
{
    private Sprite outlineSprite;
    private Color outlineColor = new Color(1f, 1f, 1f, 0.4f);
    private float outlineScale = 1.05f;

    [MenuItem("Tools/Turret Tiles/Add Outline to All Tiles")]
    public static void ShowWindow()
    {
        GetWindow<AddOutlineToTiles>("Add Outlines");
    }

    private void OnGUI()
    {
        GUILayout.Label("Add Outline to All Turret Tiles", EditorStyles.boldLabel);
        outlineSprite = (Sprite)EditorGUILayout.ObjectField("Outline Sprite", outlineSprite, typeof(Sprite), false);
        outlineColor = EditorGUILayout.ColorField("Outline Color", outlineColor);
        outlineScale = EditorGUILayout.Slider("Outline Scale", outlineScale, 1f, 1.5f);

        if (GUILayout.Button("Add / Update Outlines"))
        {
            AddOutlines();
        }
    }

    private void AddOutlines()
    {
        if (outlineSprite == null)
        {
            EditorUtility.DisplayDialog("Missing Sprite", "Please assign an Outline Sprite first!", "OK");
            return;
        }

        int count = 0;
        TurretTile[] tiles = FindObjectsOfType<TurretTile>();

        foreach (var tile in tiles)
        {
            Transform outline = tile.transform.Find("Outline");

            if (outline == null)
            {
                // Создаём новый объект
                GameObject outlineObj = new GameObject("Outline");
                outlineObj.transform.SetParent(tile.transform);
                outlineObj.transform.localPosition = Vector3.zero;
                outlineObj.transform.localRotation = Quaternion.identity;
                outlineObj.transform.localScale = Vector3.one * outlineScale;

                var sr = outlineObj.AddComponent<SpriteRenderer>();
                sr.sprite = outlineSprite;
                sr.color = outlineColor;
                sr.sortingOrder = 10; // выше обычного тайла
                sr.enabled = false;   // выключен по умолчанию
            }
            else
            {
                // Обновляем существующий контур
                var sr = outline.GetComponent<SpriteRenderer>();
                if (sr == null)
                    sr = outline.gameObject.AddComponent<SpriteRenderer>();

                sr.sprite = outlineSprite;
                sr.color = outlineColor;
                sr.sortingOrder = 10;
                outline.localScale = Vector3.one * outlineScale;
            }

            count++;
        }

        EditorUtility.DisplayDialog("Outlines Added", $"Добавлено или обновлено контуров: {count}", "OK");
    }
}
