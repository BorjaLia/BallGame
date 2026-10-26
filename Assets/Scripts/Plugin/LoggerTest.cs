using UnityEngine;
using System.Collections.Concurrent;
using TMPro;

public class LoggerTest : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI logDisplayUI;

#if UNITY_ANDROID
    private AndroidJavaObject loggerManager;
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();

    void Start()
    {
        using (var cls = new AndroidJavaClass("com.lia.logger.LoggerManager"))
        {
            loggerManager = cls.CallStatic<AndroidJavaObject>("Get");
        }

        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            loggerManager.Call("Init", activity);
        }

        Application.logMessageReceivedThreaded += HandleLog;

        Debug.Log("Logger Manager Inicializado!");
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = $"[{System.DateTime.Now:HH:mm:ss}] [{type}] {logString}";
        logQueue.Enqueue(formattedLog);
    }

    void Update()
    {
        while (logQueue.TryDequeue(out string logMsg))
        {
            loggerManager?.Call("SendLog", logMsg);
        }
    }

    public void RefreshLogsUI()
    {
        if (loggerManager != null && logDisplayUI != null)
        {
            string allLogs = loggerManager.Call<string>("GetAllLogs");

            logDisplayUI.text = allLogs;
        }
    }

    public void CallClearLogs()
    {
        loggerManager?.Call("RequestClearLogs");

        if (logDisplayUI != null) logDisplayUI.text = "";
    }

    public void LogCurrentTime()
    {
        if (loggerManager != null)
        {
            long javaTime = loggerManager.Call<long>("GetTime");

            Debug.Log($"Log manual - Hora nativa de Android (ms): {javaTime}");
        }
    }

    private void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
        loggerManager?.Dispose();
    }
#endif
}