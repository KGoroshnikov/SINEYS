using UnityEngine;
using System.Collections;

public class AABB2DScreenOutline : MonoBehaviour
{
    [HideInInspector] public GameObject currentObject;

    [Header("Outline Settings")]
    public Color lineColor = Color.white;
    public float lineThickness = 2f;
    public OutlineStyle outlineStyle = OutlineStyle.Full;
    public float cornerLength = 20f;

    [Header("Shadow Settings")]
    public bool enableShadow = true;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(3f, 3f);
    public int shadowSoftness = 2;

    [Header("Fade & Scale Settings")]
    public float fadeDuration = 0.1f;
    public float scaleMin = 0.25f;
    public float scaleMax = 1.5f;

    private Camera cam;
    private static Texture2D _lineTex;
    private Vector3[] worldCorners = new Vector3[8];
    private Vector2[] screenCorners = new Vector2[4];

    private Coroutine fadeRoutine;
    private float currentAlpha = 1f;
    private float currentScale = 1f;

    void Start()
    {
        cam = Camera.main;
        G.aabb = this;
    }

    public void SetTarget(GameObject obj)
    {
        currentObject = obj;
        if (obj != null)
        {
            enabled = true;
            ShowOutline();
        }
        else
        {
            HideOutline();
        }
    }

    public void HideOutline()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutline(0f, scaleMin, true));
    }

    public void ShowOutline()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutline(1f, scaleMax));
    }

    IEnumerator FadeOutline(float targetAlpha, float targetScale, bool hide = false)
    {
        float startAlpha = currentAlpha;
        float startScale = currentScale;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            currentScale = Mathf.Lerp(startScale, targetScale, t);
            yield return null;
        }

        currentAlpha = targetAlpha;
        currentScale = targetScale;
        if (hide)
        {
            currentObject = null;
        }
    }

    void OnGUI()
    {
        if (currentObject == null || cam == null) return;
        Renderer rend = currentObject.GetComponent<Renderer>();
        if (rend == null) return;
        if (currentAlpha <= 0.01f) return;

        Bounds bounds = rend.bounds;
        Vector3[] wc = worldCorners;

        wc[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        wc[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        wc[2] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        wc[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        wc[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        wc[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        wc[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
        wc[7] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);

        Vector2 screenMin = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 screenMax = new Vector2(float.MinValue, float.MinValue);
        bool anyVisible = false;

        for (int i = 0; i < wc.Length; i++)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(wc[i]);
            if (screenPoint.z < 0) continue;

            Vector2 sp = new Vector2(screenPoint.x, Screen.height - screenPoint.y);
            screenMin = Vector2.Min(screenMin, sp);
            screenMax = Vector2.Max(screenMax, sp);
            anyVisible = true;
        }

        if (!anyVisible) return;

        screenCorners[0] = new Vector2(screenMin.x, screenMin.y);
        screenCorners[1] = new Vector2(screenMax.x, screenMin.y);
        screenCorners[2] = new Vector2(screenMax.x, screenMax.y);
        screenCorners[3] = new Vector2(screenMin.x, screenMax.y);

        Vector2 center = (screenMin + screenMax) / 2f;

        for (int i = 0; i < screenCorners.Length; i++)
        {
            screenCorners[i] = center + (screenCorners[i] - center) * currentScale;
        }

        if (_lineTex == null)
            _lineTex = new Texture2D(1, 1);

        Color lineCol = new Color(lineColor.r, lineColor.g, lineColor.b, lineColor.a * currentAlpha);
        Color shadowCol = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowColor.a * currentAlpha);

        // Тень
        if (enableShadow)
        {
            for (int i = 1; i <= shadowSoftness; i++)
            {
                float offsetFactor = i / (float)shadowSoftness;
                Vector2 offset = shadowOffset * offsetFactor;
                DrawOutline(screenCorners, shadowCol, offset);
            }
        }
        DrawOutline(screenCorners, lineCol, Vector2.zero);
    }

    void DrawOutline(Vector2[] corners, Color color, Vector2 offset)
    {
        _lineTex.SetPixel(0, 0, color);
        _lineTex.Apply();

        if (outlineStyle == OutlineStyle.Full)
        {
            DrawLine(corners[0] + offset, corners[1] + offset);
            DrawLine(corners[1] + offset, corners[2] + offset);
            DrawLine(corners[2] + offset, corners[3] + offset);
            DrawLine(corners[3] + offset, corners[0] + offset);
        }
        else if (outlineStyle == OutlineStyle.Corners)
        {
            DrawCorner(corners[0] + offset, Vector2.right);
            DrawCorner(corners[0] + offset, Vector2.up);
            DrawCorner(corners[2] + offset, Vector2.left);
            DrawCorner(corners[2] + offset, Vector2.down);
            //DrawCorner(corners[1] + offset, Vector2.left);
            //DrawCorner(corners[1] + offset, Vector2.up);
            //DrawCorner(corners[3] + offset, Vector2.right);
            //DrawCorner(corners[3] + offset, Vector2.down);
        }
    }

    void DrawLine(Vector2 p1, Vector2 p2)
    {
        float distance = Vector2.Distance(p1, p2);
        if (distance < 0.01f) return;

        Vector2 dir = (p2 - p1).normalized;
        var matrixBackup = GUI.matrix;
        GUIUtility.RotateAroundPivot(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, p1);
        GUI.DrawTexture(new Rect(p1.x, p1.y, distance, lineThickness), _lineTex);
        GUI.matrix = matrixBackup;
    }

    void DrawCorner(Vector2 origin, Vector2 direction)
    {
        Vector2 end = origin + direction * cornerLength;
        DrawLine(origin, end);
    }
}

public enum OutlineStyle
{
    Full,
    Corners
}
