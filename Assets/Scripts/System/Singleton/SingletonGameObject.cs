using UnityEngine;

public class SingletonGameObject : MonoBehaviour
{
    SingletonGameObject()
    {
        Debug.Log("Add GO");

        GameObject gameObjectHandle = new GameObject();
        gameObjectHandle.name = typeof(T).ToString();
        UnityEngine.Object.DontDestroyOnLoad(gameObjectHandle);

        T service = gameObjectHandle.AddComponent<T>();

        AddService(service);

        if (function == null) return;

        if (param == null)
        {
            Debug.LogWarning($"{gameObjectHandle.name} had null params!");
            return;
        }

        if (function(param))
        {
            Debug.LogWarning($"{gameObjectHandle.name} couldn't run function");
        }
        return;
    }
}
