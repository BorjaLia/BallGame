using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementHandler movementScript;
    [SerializeField]
    private PlayerInputHandler inputScript;

    public PlayerMovementHandler MovementScript { get { return movementScript; } private set { } }
    public PlayerInputHandler InputScript { get { return inputScript; } private set { } }

    [Header("Stats")]
    [SerializeField] private PlayerStats stats = PlayerStats.Default;

    private EventBus eventBus;
    private Rigidbody rb;

    private int iceTileCount = 0;
    private float defaultLinearDamping;
    private float currentControlMultiplier = 1f;

    public float ControlMultiplier { get { return currentControlMultiplier; } private set { } }

    void Start()
    {
        FindScriptReferences();

        rb = GetComponent<Rigidbody>();
        defaultLinearDamping = rb.linearDamping;

        eventBus = ServiceLocator.Instance.Get<EventBus>();
        if (eventBus != null)
        {
            eventBus.Subscribe<SpeedBoostTriggeredEvent>(HandleSpeedBoost);
            eventBus.Subscribe<IceTileEnteredEvent>(HandleIceEnter);
            eventBus.Subscribe<IceTileExitedEvent>(HandleIceExit);
        }
    }

    private void FindScriptReferences()
    {
        if (!movementScript)
        {
            movementScript = GetComponent<PlayerMovementHandler>();
        }
        if (!movementScript)
        {
            Debug.LogError("no movement script attached on player");
        }

        if (!inputScript)
        {
            inputScript = GetComponent<PlayerInputHandler>();
        }
        if (!inputScript)
        {
            Debug.LogError("no movement script attached on player");
        }
    }

    private void OnDestroy()
    {
        if (eventBus != null)
        {
            eventBus.Unsubscribe<SpeedBoostTriggeredEvent>(HandleSpeedBoost);
            eventBus.Unsubscribe<IceTileEnteredEvent>(HandleIceEnter);
            eventBus.Unsubscribe<IceTileExitedEvent>(HandleIceExit);
        }
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        if (movementScript == null)
        {
            movementScript = GetComponent<PlayerMovementHandler>();
        }

        movementScript?.SetMovementEnabled(isEnabled);
    }
    private void HandleSpeedBoost(SpeedBoostTriggeredEvent e)
    {
        if (rb == null) return;

        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 boostDir = horizontalVel.sqrMagnitude > 0.01f ? horizontalVel.normalized : transform.forward;

        float finalBoost = e.BoostForce * stats.speedBoostMultiplier;
        rb.AddForce(boostDir * finalBoost, ForceMode.VelocityChange);
    }

    private void HandleIceEnter(IceTileEnteredEvent e)
    {
        iceTileCount++;

        float effectiveDampingMult = Mathf.Lerp(e.DampingMultiplier, 1f, stats.iceGrip);
        float effectiveControlMult = Mathf.Lerp(e.ControlMultiplier, 1f, stats.iceGrip);

        rb.linearDamping = defaultLinearDamping * effectiveDampingMult;
        currentControlMultiplier = effectiveControlMult;
    }

    private void HandleIceExit(IceTileExitedEvent e)
    {
        iceTileCount = Mathf.Max(0, iceTileCount - 1);

        if (iceTileCount == 0)
        {
            rb.linearDamping = defaultLinearDamping;
            currentControlMultiplier = 1f;
        }
    }
}
