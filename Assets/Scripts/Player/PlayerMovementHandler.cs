using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxSpeed = 10.0f;
    private float maxSpeedSqr = 0.0f;
    
    [Header("Input Action References")]
    [SerializeField] private PlayerHandler controller;
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speedDebug;
    [SerializeField] private Vector3 dir;
    [SerializeField] public float forceMult = 1.0f;
   

    void Awake()
    {
        maxSpeedSqr = maxSpeed * maxSpeed;
    }

    void Start()
    {
        FindScriptReferences();
    }
    private void FindScriptReferences()
    {
        if (!controller)
        {
            controller = GetComponent<PlayerHandler>();
            if (!controller)
            {
                Debug.LogError("No controller found");
            }
        }

        if (!inputHandler)
        {
            if (controller) inputHandler = controller.InputScript;
        }

        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                Debug.LogError("No rigidbody found");
            }
        }
    }

    private void FixedUpdate()
    {
        dir = new Vector3(inputHandler.MoveInput.x,0.0f,inputHandler.MoveInput.y);
        
        rb.AddForce(dir * forceMult * Time.fixedDeltaTime, ForceMode.Force);
        
        if (rb.linearVelocity.sqrMagnitude >= maxSpeedSqr)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        speedDebug = rb.linearVelocity.magnitude;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        enabled = isEnabled;

        if (rb != null)
        {
            if (!isEnabled && !rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            rb.isKinematic = !isEnabled;
        }
    }
}