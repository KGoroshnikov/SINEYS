using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class RayCast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 2.5f;  
    public GameObject crosshair;
    [SerializeField] private TextMeshProUGUI text;
    public bool objectAnchor;
    private Canvas canvas;

    public Texture[] interactIcons;

    private GameObject hitObj = null;
    public void Awake()
    {
        G.raycast = this;
        canvas = crosshair.GetComponentInParent<Canvas>();
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance) && hit.collider.gameObject.GetComponent<Interactable>())
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                GameObject obj = hit.collider.gameObject;
                if (obj.GetComponent<IUsable>() != null)
                {
                    obj.GetComponent<IUsable>().Use();
                }
            }

            if (hitObj != null)
            {
                Outline(false);
                hitObj = null;
            }
            crosshair.SetActive(true);
            if(interactIcons.Length > 0)
                crosshair.GetComponent<RawImage>().texture = interactIcons[hit.collider.gameObject.GetComponent<Interactable>().iconId];
            if (text != null)
                text.text = hit.collider.gameObject.GetComponent<Interactable>().title;
            hitObj = hit.collider.gameObject;

            Outline(true);

            if (objectAnchor)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(hitObj.transform.position);
                if (hitObj.GetComponent<Interactable>().point != null)
                    screenPos = Camera.main.WorldToScreenPoint(hitObj.GetComponent<Interactable>().point.position);

                Vector2 uiPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    screenPos,
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                    out uiPos
                );

                crosshair.GetComponent<RectTransform>().anchoredPosition = uiPos;
            }
        }
        else
        {
            crosshair.SetActive(false);
            text.text = "";
            if (hitObj != null)
            {
                Outline(false);
                hitObj = null;
            }
        }
    }

    public void Outline(bool enabled = false)
    {
        if (enabled)
        {
            G.aabb.currentObject = hitObj;
            G.aabb.ShowOutline();
        }
        else
        {
            G.aabb.HideOutline();
        }
    }
}

public interface IUsable
{
    public void Use();
}