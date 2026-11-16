using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;

public class Pool<T> where T :IPoolObject
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly Func<T> createObject;

    public Pool(Func<T> createObject, int initialSize = 0)
    {
        this.createObject = createObject;

        for (int i = 0; i < initialSize; i++)
        {
            pool.Enqueue(NewObject());
        }
    }

    public T Get()
    {
        T item = pool.Count > 0 ? pool.Dequeue() : NewObject();

        item.OnSpawn();

        return item;
    }

    public void Return(T item)
    {
        item.OnDespawn();

        pool.Enqueue(item);
    }

    private T NewObject()
    {
        return createObject();
    }
}
