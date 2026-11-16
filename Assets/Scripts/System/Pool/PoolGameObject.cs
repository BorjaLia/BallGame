using UnityEngine;
public class ComponentPool<T> where T : Component, IPoolObject
{
    private readonly Pool<T> pool;
    private readonly Transform containerTransform;

    public ComponentPool(T prefab, int initialSize, Transform rootParent = null)
    {
        GameObject containerGo = new GameObject($"Pool_{prefab.name}");
        containerTransform = containerGo.transform;

        if (rootParent != null)
        {
            containerTransform.SetParent(rootParent);
        }

        pool = new Pool<T>(
            createObject: () =>
            {
                T obj = Object.Instantiate(prefab, containerTransform);
                obj.gameObject.SetActive(false);
                return obj;
            },
            initialSize: initialSize
        );
    }

    public T Get()
    {
        T obj = pool.Get();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Return(obj);
    }
}