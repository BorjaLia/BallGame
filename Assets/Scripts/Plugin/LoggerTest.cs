using UnityEngine;

public class LoggerTest : MonoBehaviour
{
#if UNITY_ANDROID

    static int calls = 0;

    public AndroidJavaObject loggerManager;

    void Start()
    {
        Debug.Log("Start LoggerManager");

        using var cls = new AndroidJavaClass("com.lia.logger.LoggerManager");
        loggerManager = cls.CallStatic<AndroidJavaObject>("Get");
    }

    public void CallPlugin()
    {
        calls++;

        loggerManager.Call("SendLog", "Hola desde unity");

        long t = loggerManager.Call<long>("GetTime");
        Debug.Log($"Hora: {t}");
        Debug.Log($"Call: {calls}");
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
