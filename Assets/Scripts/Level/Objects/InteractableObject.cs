using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected Collider col;
    [SerializeField] protected string playerTag;

    protected EventBus eventBus;

    protected Action onPlayerEnterAction = null;
    protected Action onPlayerExitAction = null;

    protected virtual void Awake()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        if (!eventBus)
        {
            Debug.LogError("No event bus found!");
        }
    }

    protected virtual void Start()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            onPlayerEnterAction?.Invoke();
            Debug.Log("Player entered special tile!");
        }
        else
        {
            Debug.Log("Something entered special tile!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            onPlayerExitAction?.Invoke();
            Debug.Log("Player exited special tile!");
        }
        else
        {
            Debug.Log("Something exited special tile!");
        }
    }
}
