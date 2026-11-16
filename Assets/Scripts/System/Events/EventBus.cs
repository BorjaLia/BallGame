using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    private Dictionary<Type, List<Delegate>> handles = new Dictionary<Type, List<Delegate>>();

    void Awake()
    {
        Debug.Log("Event Bus Initialized");
    }

    public void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        Type eventType = typeof(T);
        List<Delegate> delegates;

        if (!handles.TryGetValue(eventType, out delegates))
        {
            delegates = new List<Delegate>();
            handles.Add(eventType, delegates);
        }

        delegates.Add(handler);
        Debug.Log("Subscribed event of type " + eventType.Name);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        Type eventType = typeof(T);

        if (handles.TryGetValue(eventType, out List<Delegate> delegates))
        {
            delegates.Remove(handler);
            Debug.Log("Unsubscribed event of type " + eventType.Name);
        }
    }

    public void Publish<T>(T eventData) where T : IEvent
    {
        var eventType = typeof(T);

        if (handles.TryGetValue(eventType, out List<Delegate> delegates))
        {
            foreach (var handler in delegates.ToList())
            {
                (handler as Action<T>)?.Invoke(eventData);
            }
        }
        else
        {
            Debug.LogWarning("Event has not been published, no subscribers!");
            return;
        }
    }
}
