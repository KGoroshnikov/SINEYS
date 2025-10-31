using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CCursor : MonoBehaviour
{
    [Header("Icons")]
    public Texture defaultIcon;
    public Texture hoverIcon;

    [HideInInspector] public bool onUI;
    [HideInInspector] public RectTransform canvasRectTransform;
    [HideInInspector] public RawImage icon;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private void Awake()
    {
        Cursor.visible = false;

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
            onUI = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay;
            raycaster = parentCanvas.GetComponent<GraphicRaycaster>();
        }

        icon = GetComponentInChildren<RawImage>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Update()
    {
        if (canvasRectTransform == null) return;

        Cursor.visible = false;

        if (!onUI)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Plane canvasPlane = new Plane(canvasRectTransform.forward, canvasRectTransform.position);

            if (canvasPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                transform.position = worldPos;
            }
        }
        else
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 worldPoint;

            Canvas canvas = canvasRectTransform.GetComponent<Canvas>();
            Camera cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;
            else if (canvas.renderMode == RenderMode.WorldSpace)
                cam = Camera.main;

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, mousePosition, cam, out worldPoint))
            {
                GetComponent<RectTransform>().position = worldPoint;
            }
        }

        if (raycaster != null && eventSystem != null && icon != null)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0 && results[0].gameObject.GetComponent<Button>() != null)
                icon.texture = hoverIcon;
            else
                icon.texture = defaultIcon;
        }
    }
}
