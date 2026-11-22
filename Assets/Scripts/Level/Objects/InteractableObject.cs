using System;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsComposer;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected TagHandle playerTag;
    [SerializeField] protected Collider col;

    private EventBus eventBus;

    protected Action onPlayerCollisionAction = null;
    protected IEvent onPlayerCollisionEvent = null;

    void Awake()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        if (!eventBus)
        {
            Debug.LogError("No event bus found!");
        }
    }

    void Start()
    {
        FindScriptReferences();
    }

    private void FindScriptReferences()
    {
        if (!col)
        {
            col = GetComponent<Collider>();
            if (!col)
            {
                Debug.LogError("No collider found!");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            if(onPlayerCollisionAction != null) onPlayerCollisionAction?.DynamicInvoke(collision.gameObject);
            if (onPlayerCollisionEvent != null) eventBus.Publish(onPlayerCollisionEvent);
        }
    }
}
