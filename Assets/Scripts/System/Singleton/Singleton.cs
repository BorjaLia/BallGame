using UnityEngine;

public class Singleton<T> where T : class, new()
{
    private static T _instance = null;
    
    public static T Instance 
    {
        get {
            if (_instance == null)
            {
                _instance = new T();
            }
            else
            {
                Debug.LogWarning("Instanced " + typeof(T).ToString() + " as singleton");
            }
            return _instance; 
        }
    }
}
