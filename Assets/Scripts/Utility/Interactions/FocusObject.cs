using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class FocusObject : MonoBehaviour, IUsable
{
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [SerializeField] private bool cursor;
    [SerializeField] private Transform target;
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] private float returnSpeed = 2.0f;
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private bool isMoving = false;
    private bool movingToTarget = false;
    private bool returningToOriginal = false;

    private Vector3 currentCameraPosition;
    private Quaternion currentCameraRotation;

    public bool cantBack;
    public bool useEvents;
    [ShowIf("useEvents")] public UnityEvent onEnterStart;
    [ShowIf("useEvents")] public UnityEvent onEnter;
    [ShowIf("useEvents")] public UnityEvent onExitStart;
    [ShowIf("useEvents")] public UnityEvent onExit;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) && isMoving && !returningToOriginal && !cantBack)
        {

            currentCameraPosition = mainCamera.transform.position;
            currentCameraRotation = mainCamera.transform.rotation;
            StartCoroutine(ReturnToOriginal());
        }
    }

    public void UnZoom()
    {
        currentCameraPosition = mainCamera.transform.position;
        currentCameraRotation = mainCamera.transform.rotation;
        StartCoroutine(ReturnToOriginal());
    }

    public void Use()
    {
        if (!isMoving && !G.gm.cantZoom)
        {
            G.gm.cantEsc = true;
            isMoving = true;
            originalPosition = mainCamera.transform.position;
            originalRotation = mainCamera.transform.rotation;
            G.rigidcontroller.enabled = false;
            G.rigidcontroller.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        Camera.main.GetComponent<HeadBob>().enabled = false;
        onEnterStart.Invoke();
        if (GetComponent<Collider>())
            GetComponent<Collider>().enabled = false;
        movingToTarget = true;
        returningToOriginal = false;

        float journey = 0f;

        while (movingToTarget && mainCamera != null)
        {
            journey += Time.deltaTime * transitionSpeed;

            float curveValue = speedCurve.Evaluate(journey);


            mainCamera.transform.position = Vector3.Lerp(originalPosition, target.position, curveValue);

            mainCamera.transform.rotation = Quaternion.Lerp(originalRotation, target.rotation, curveValue);

            if (Vector3.Distance(mainCamera.transform.position, target.position) < 0.01f && Quaternion.Angle(mainCamera.transform.rotation, target.rotation) < 0.01f)
            {

                onEnter.Invoke();
                movingToTarget = false;
                if (cursor)
                {
                    G.ShowCursor();
                }

            }

            yield return null;
        }
    }

    IEnumerator ReturnToOriginal()
    {
        yield return new WaitForSeconds(0.01f);
        onExitStart.Invoke();
        movingToTarget = false;
        returningToOriginal = true;

        float journey = 0f;

        if (cursor)
        {
            G.HideCursor();
        }

        while (returningToOriginal && mainCamera != null)
        {
            journey += Time.deltaTime * returnSpeed;

            float curveValue = speedCurve.Evaluate(journey);


            mainCamera.transform.position = Vector3.Lerp(currentCameraPosition, originalPosition, curveValue);

            mainCamera.transform.rotation = Quaternion.Lerp(currentCameraRotation, originalRotation, curveValue);

            if (Vector3.Distance(mainCamera.transform.position, originalPosition) < 0.01f && Quaternion.Angle(mainCamera.transform.rotation, originalRotation) < 0.01f)
            {
                G.gm.cantEsc = false;
                returningToOriginal = false;
                G.rigidcontroller.enabled = true;
                G.rigidcontroller.GetComponent<Rigidbody>().isKinematic = false;
                isMoving = false;
                onExit.Invoke();
                if (GetComponent<Collider>())
                    GetComponent<Collider>().enabled = true;
            }

            yield return null;
        }
        Camera.main.GetComponent<HeadBob>().enabled = true;
    }
}
