using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        Debug.Log("Initialized bootstrapper");

        ServiceLocator sl = ServiceLocator.Get();
    }
}