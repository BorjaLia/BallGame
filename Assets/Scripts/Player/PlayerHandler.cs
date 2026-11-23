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
    [SerializeField] private Renderer marbleRenderer;

    private EventBus eventBus;
    private Rigidbody rb;

    public int CurrentLives { get; private set; }
    public int MaxLives => stats.maxLives;

    private int iceTileCount = 0;
    private float defaultLinearDamping;
    private float currentControlMultiplier = 1f;

    public float ControlMultiplier { get { return currentControlMultiplier; } private set { } }

    void Start()
    {
        FindScriptReferences();

        PlayerProgressManager progressManager = ServiceLocator.Instance.Get<PlayerProgressManager>();
        
        if (progressManager != null)
        {
            stats = progressManager.GetCalculatedStats();
            ApplyCosmeticColor(progressManager.GetSelectedColor());
        }

        CurrentLives = Mathf.Max(1, stats.maxLives);
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

        rb = GetComponent<Rigidbody>();
        defaultLinearDamping = rb.linearDamping;

        if (marbleRenderer == null)
        {
            marbleRenderer = GetComponentInChildren<Renderer>();
        }

        eventBus = ServiceLocator.Instance.Get<EventBus>();
        if (eventBus != null)
        {
            eventBus.Subscribe<SpeedBoostTriggeredEvent>(HandleSpeedBoost);
            eventBus.Subscribe<IceTileEnteredEvent>(HandleIceEnter);
            eventBus.Subscribe<IceTileExitedEvent>(HandleIceExit);
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

    public void LoseLife()
    {
        CurrentLives = Mathf.Max(0, CurrentLives - 1);
        eventBus?.Publish(new PlayerLivesChangedEvent(CurrentLives));
    }

    public void AddLife(int amount = 1)
    {
        if (amount <= 0) return;

        CurrentLives += amount;
        eventBus?.Publish(new PlayerLivesChangedEvent(CurrentLives));
    }

    public void ResetTileEffects()
    {
        iceTileCount = 0;
        currentControlMultiplier = 1f;

        if (rb != null)
            rb.linearDamping = defaultLinearDamping;
    }

    private void ApplyCosmeticColor(Color color)
    {
        if (marbleRenderer != null)
            marbleRenderer.material.color = color;
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
