using NaughtyAttributes;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;

    [HideIf("useCollider")] public float minDistance = 4f;
    [HideIf("useCollider")] public float maxDistance = 10f;

    public bool move;
    [ShowIf("move")] public float maxMoveSpeed = 3f;
    [ShowIf("move")] public float stopDistance = 1f;

    public bool rotate;
    [ShowIf("rotate")] public float rotationSpeed = 5f;
    [ShowIf("rotate")] public float maxCameraAngleForLook = 60f;

    public bool useCollider = false;

    private Transform player;
    private Transform cameraTransform;
    private bool playerInTrigger = false;

    private void Start()
    {
        player = G.gm.player.transform;
        cameraTransform = Camera.main.transform;

        if (target == null) target = transform;
        if (lookTarget == null) lookTarget = transform;
    }

    private void Update()
    {
        if (player == null || cameraTransform == null)
            return;

        Vector3 origin = target != null ? target.position : transform.position;
        Vector3 flatTargetPos = new Vector3(lookTarget.position.x, player.position.y, lookTarget.position.z);
        float distance = Vector3.Distance(player.position, origin);

        bool withinRadius = distance <= maxDistance;
        bool allowAttract = (!useCollider && withinRadius) || (useCollider && playerInTrigger && withinRadius);

        var controller = player.GetComponent<RigidbodyFirstPersonController>();
        if (controller == null || controller.mouseLook == null)
            return;

        var mouseLook = controller.mouseLook;

        if (!allowAttract || distance <= stopDistance)
        {
            mouseLook.externalCharacterRotationOffset = Quaternion.identity;
            mouseLook.externalCameraRotationOffset = Quaternion.identity;
            return;
        }

        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float influence = Mathf.Clamp01(t);

        Vector3 directionToTarget = (flatTargetPos - player.position).normalized;

        if (rotate)
        {
            float angleBetween = Vector3.Angle(player.forward, directionToTarget);
            if (angleBetween <= maxCameraAngleForLook)
            {
                Vector3 localTarget = cameraTransform.InverseTransformPoint(lookTarget.position);
                float angleX = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;

                Quaternion camDelta = Quaternion.Euler(angleX, 0f, 0f);
                Quaternion camInfluence = Quaternion.Slerp(Quaternion.identity, camDelta, influence * rotationSpeed * Time.deltaTime);
                mouseLook.externalCameraRotationOffset = camInfluence;
            }
            else
            {
                mouseLook.externalCameraRotationOffset = Quaternion.identity;
            }

            Vector3 flatDir = new Vector3(directionToTarget.x, 0f, directionToTarget.z);
            Quaternion delta = Quaternion.FromToRotation(player.forward, flatDir);
            Quaternion influenceRotation = Quaternion.Slerp(Quaternion.identity, delta, influence * rotationSpeed * Time.deltaTime);
            mouseLook.externalCharacterRotationOffset = influenceRotation;
        }

        if (move && distance > stopDistance)
        {
            float speed = maxMoveSpeed * influence;

            Vector3 targetDir = (flatTargetPos - player.position).normalized;
            Vector3 targetPoint = flatTargetPos - targetDir * stopDistance;

            Vector3 moveTarget = Vector3.MoveTowards(player.position, targetPoint, speed * Time.deltaTime);
            moveTarget.y = player.position.y;

            player.position = moveTarget;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!useCollider)
        {
            Vector3 origin = target != null ? target.position : transform.position;

            Gizmos.color = new Color(1f, 1f, 0f, 0.7f);
            Gizmos.DrawWireSphere(origin, maxDistance);

            Gizmos.color = new Color(0f, 1f, 0f, 0.7f);
            Gizmos.DrawWireSphere(origin, minDistance);
        }
    }
}
