using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RigidbodyFirstPersonController : MonoBehaviour
{
    void Awake()
    {
        G.rigidcontroller = this;
    }

    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8.0f;
        public float BackwardSpeed = 4.0f;
        public float StrafeSpeed = 4.0f;
        public float RunMultiplier = 2.0f;
        public float croachSpeedMultiplier = 0.5f;
        public float JumpForce = 30f;
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        public bool cantRun;
        public bool cantJump;
        public bool cantCroach;
        [HideInInspector] public float CurrentTargetSpeed;
        [HideInInspector] public bool m_Running;
        [HideInInspector] public bool m_Croaching;

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.y > 0)
            {
                CurrentTargetSpeed = ForwardSpeed;
            }
            if(m_Croaching)
            {
                CurrentTargetSpeed *= croachSpeedMultiplier;
            }

            if (cantRun == false)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    CurrentTargetSpeed *= RunMultiplier;
                    m_Running = true;
                }
                else
                {
                    m_Running = false;
                }
            }

        }

        public bool Running
        {
            get { return m_Running; }
        }
    }

    [Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.01f;
        public float stickToGroundHelperDistance = 0.5f;
        public float slowDownRate = 20f;
        public bool airControl;
        public float airSpeedMult = 0.5f;
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset;

    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();
    public AdvancedSettings advancedSettings = new AdvancedSettings();


    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    [HideInInspector] public bool m_Jump;
    private bool m_PreviouslyGrounded, m_Jumping;
    [HideInInspector] public bool m_IsGrounded;

    private bool onLadder = false;
    private Ladder currentLadder;
    [SerializeField] private float ladderClimbSpeed = 4f;
    [SerializeField] private float ladderSlideDownSpeed = 2f;


    public Vector3 Velocity
    {
        get { return m_RigidBody.linearVelocity; }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    public bool Jumping
    {
        get { return m_Jumping; }
    }

    public bool Running
    {
        get
        {
            return movementSettings.Running;
        }
    }


    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        mouseLook.Init(transform, cam.transform);
    }
    private bool IsObstacleAbove()
    {
        Vector3 origin = transform.position + Vector3.up * (m_Capsule.height * 0.5f);

        float checkDistance = m_Capsule.height * 0.25f + 0.1f;

        return Physics.Raycast(origin, Vector3.up, checkDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
    }
    private void OnDrawGizmosSelected()
    {
        if (m_Capsule == null) return;
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + Vector3.up * (m_Capsule.height * 0.5f);
        Gizmos.DrawLine(origin, origin + Vector3.up * (m_Capsule.height * 0.5f + 0.1f));
    }

    private void Update()
    {
        RotateView();

        if (Input.GetButtonDown("Jump") && !m_Jump && !movementSettings.cantJump && (!movementSettings.m_Croaching || !IsObstacleAbove()))
        {
            m_Jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !GetComponent<Animation>().isPlaying)
        {
            if (!movementSettings.cantCroach && !movementSettings.m_Croaching)
            {
                movementSettings.m_Croaching = true;
                Croach();
            }
            else if (movementSettings.m_Croaching || movementSettings.cantCroach && !GetComponent<Animation>().isPlaying)
            {
                if (!IsObstacleAbove())
                {
                    movementSettings.m_Croaching = false;
                    UnCroach();
                }
            }
        }
        if(movementSettings.m_Croaching && Input.GetKey(KeyCode.LeftShift) && !GetComponent<Animation>().isPlaying && !IsObstacleAbove())
        {
            movementSettings.m_Croaching = false;
            UnCroach();
        }
    }
    public void Croach()
    {
        GetComponent<Animation>().Play("croach");
    }
    public void UnCroach()
    {
        GetComponent<Animation>().Play("unCroach");
        Delay.InvokeDelayed(() => transform.Rotate(new Vector3(0, 0.01f, 0)), GetComponent<Animation>()["unCroach"].length / 4);
        Delay.InvokeDelayed(() => transform.Rotate(new Vector3(0, 0.01f, 0)), GetComponent<Animation>()["unCroach"].length/2);
        Delay.InvokeDelayed(() => transform.Rotate(new Vector3(0,0.01f,0)), GetComponent<Animation>()["unCroach"].length);
    }

    public void OnLadderEnter(Ladder ladder)
    {
        onLadder = true;
        currentLadder = ladder;
        m_RigidBody.useGravity = false;
        m_RigidBody.linearDamping = 4f;
    }

    public void OnLadderExit(Ladder ladder)
    {
        if (currentLadder == ladder)
        {
            onLadder = false;
            currentLadder = null;
            m_RigidBody.useGravity = true;
            m_RigidBody.linearDamping = 0f;

            m_RigidBody.AddForce(transform.forward * 12f, ForceMode.Impulse);
        }
    }

    public void ReInitMouseLook()
    {
        mouseLook.Init(transform, Camera.main.transform);
    }

    private void HandleLadderMovement()
    {
        Vector2 input = GetInput();
        float vertical = input.y;

        bool isGroundBelow = Physics.Raycast(transform.position, Vector3.down, out _, 1.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        bool isCeilingAbove = Physics.Raycast(transform.position + Vector3.up * (m_Capsule.height / 2f), Vector3.up, out _, 0.2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        Vector3 move = Vector3.zero;
        move += Vector3.up * vertical * ladderClimbSpeed;
        move += cam.transform.right * input.x * 0.5f;

        m_RigidBody.linearVelocity = move;

        if (isGroundBelow && vertical < 0f)
        {
            OnLadderExit(currentLadder);
            return;
        }

        if (isCeilingAbove && vertical > 0f)
        {
            OnLadderExit(currentLadder);
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            m_RigidBody.AddForce(-transform.forward * 4f + Vector3.up * 3f, ForceMode.Impulse);
            OnLadderExit(currentLadder);
            return;
        }

        if (Mathf.Abs(vertical) < 0.1f && isGroundBelow)
        {
            OnLadderExit(currentLadder);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (onLadder)
        {
            HandleLadderMovement();
            return;
        }

        GroundCheck();

        Vector2 input = GetInput();


        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
        {
            Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;

            float airMult = (m_IsGrounded == false && advancedSettings.airControl == true) ? advancedSettings.airSpeedMult : 1;

            desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized * airMult;

            desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
            if (m_RigidBody.linearVelocity.sqrMagnitude <
                (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }
        }

        if (m_IsGrounded)
        {
            m_RigidBody.linearDamping = 5f;

            if (m_Jump)
            {
                m_RigidBody.linearDamping = 0f;
                m_RigidBody.linearVelocity = new Vector3(m_RigidBody.linearVelocity.x, 0f, m_RigidBody.linearVelocity.z);
                m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                m_Jumping = true;
            }

            if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.linearVelocity.magnitude < 1f)
            {
                m_RigidBody.Sleep();
            }
        }
        else
        {
            m_RigidBody.linearDamping = 0f;
            if (m_PreviouslyGrounded && !m_Jumping)
            {
                StickToGroundHelper();
            }
        }
        m_Jump = false;
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) +
                               advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_RigidBody.linearVelocity = Vector3.ProjectOnPlane(m_RigidBody.linearVelocity, hitInfo.normal);
            }
        }
    }

    private Vector2 GetInput()
    {

        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (m_IsGrounded || advancedSettings.airControl)
        {
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.linearVelocity = velRotation * m_RigidBody.linearVelocity;
        }
    }

    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
        }
    }
}

