using UnityEngine;

public class GameObjectPool<T> : MonoBehaviour where T : Component, IPoolObject
{
    private readonly Pool<T> pool;

    public GameObjectPool(T prefab, int initialSize)
    {
        pool = new Pool<T>(
            createObject: () =>
            {
                T obj = Object.Instantiate(prefab, transform.parent);
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