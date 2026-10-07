using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public void Register<T>() where T : class, new()
    {
        if (!_services.TryAdd(typeof(T), new T()))
        {
            Debug.LogError("could not add " + typeof(T).Name + " as a service");
        }
    }

    public void Remove<T>() where T : class, new()
    {
        if (!_services.Remove(typeof(T)))
        {
            Debug.LogError("could not remove " + typeof(T).Name + " from services");
        }
    }

    public T Get<T>() where T : class, new()
    {
        object value = null;
        if(_services.TryGetValue(typeof(T),out value))
        {
            return value as T;
        }
        else
        {
            Debug.LogError("Could not get service " + typeof(T).Name);
            return null;
        }
    }

    public bool Clear()
    {
        _services.Clear();
        return _services.Count == 0;
    }
}
