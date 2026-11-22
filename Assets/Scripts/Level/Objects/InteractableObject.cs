using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected Collider col;
    [SerializeField] protected string playerTag;

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

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if(onPlayerCollisionAction != null) onPlayerCollisionAction?.DynamicInvoke();
            if (onPlayerCollisionEvent != null) eventBus.Publish(onPlayerCollisionEvent);
        }
    }
}
