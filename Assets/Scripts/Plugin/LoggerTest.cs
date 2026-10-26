using UnityEngine;

public class LoggerTest : MonoBehaviour
{
#if UNITY_ANDROID

    public AndroidJavaObject loggerManager;

    void Start()
    {
        using var cls = new AndroidJavaClass("com.lia.logger.LoggerManager");
        loggerManager = cls.CallStatic<AndroidJavaClass>("Get");

        loggerManager.Call("SendLog","Hola desde unity");

        long t = loggerManager.Call<long>("GetTime");
        Debug.Log($"Hora: {t}");
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        loggerManager?.Dispose();
    }
#endif
}
