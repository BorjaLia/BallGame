using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private Dictionary<Type, object> _services = new Dictionary<Type, object>();
    private readonly Dictionary<Type, GameObject> _gameObjectHandles = new Dictionary<Type, GameObject>();

    public void Register<T>(T service) where T : class
    {
        if (typeof(T).IsAssignableFrom(typeof(MonoBehaviour)))
        {
            Debug.LogError("Use RegisterAsMonobehaviour<T>() to register MonoBehaviours!");
        }

        if (service == null)
        {
            Debug.LogError("A non null instance must be included!");
            return;
        }

        Type type = typeof(T);

        if (!_services.TryAdd(type, service))
        {
            Debug.LogError($"could not add {typeof(T).Name} as a service");
        }
    }

    public void RegisterAsMonobehaviour<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);

        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"{type.Name} is already registered.");
            return;
        }

        GameObject gameObjectHandle = new GameObject();
        gameObjectHandle.name = typeof(T).ToString();
        UnityEngine.Object.DontDestroyOnLoad(gameObjectHandle);

        T service = gameObjectHandle.AddComponent<T>();

        _services.Add(type, service);
        _gameObjectHandles.Add(type, gameObjectHandle);

        return;
    }

    public void Remove<T>() where T : class
    {
        Type type = typeof(T);

        if (_services.Remove(type))
        {
            if (_gameObjectHandles.TryGetValue(type, out GameObject handle))
            {
                UnityEngine.Object.Destroy(handle);
                _gameObjectHandles.Remove(type);
            }
        }
        else
        {
            Debug.LogError("could not remove " + typeof(T).Name + " from services");
        }
    }

    public T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object value))
        {
            return value as T;
        }

        Debug.LogError("Could not get service " + typeof(T).Name);
        return null;

    }

    public bool Clear()
    {
        foreach (var handle in _gameObjectHandles.Values)
        {
            if (handle != null)
            {
                UnityEngine.Object.Destroy(handle);
            }
        }

        _gameObjectHandles.Clear();
        _services.Clear();

        return (_services.Count == 0 && _gameObjectHandles.Count == 0);
    }
}
