using UnityEngine;
using System.Collections.Concurrent;
using TMPro;

public class LoggerTest : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI logDisplayUI;

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject loggerManager;
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
#endif
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
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
#endif
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string formattedLog = $"[{System.DateTime.Now:HH:mm:ss}] [{type}] {logString}";
        logQueue.Enqueue(formattedLog);
#endif
    }

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        while (logQueue.TryDequeue(out string logMsg))
        {
            loggerManager?.Call("SendLog", logMsg);
        }
#endif
    }

    public void RefreshLogsUI()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerManager != null && logDisplayUI != null)
        {
            string allLogs = loggerManager.Call<string>("GetAllLogs");

            logDisplayUI.text = allLogs;
        }
#endif
    }

    public void CallClearLogs()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        loggerManager?.Call("RequestClearLogs");

        if (logDisplayUI != null) logDisplayUI.text = "";
#endif
    }

    public void LogCurrentTime()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (loggerManager != null)
        {
            long javaTime = loggerManager.Call<long>("GetTime");

            Debug.Log($"Log manual - Hora nativa de Android (ms): {javaTime}");
        }
#endif
    }

    private void OnDestroy()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.logMessageReceivedThreaded -= HandleLog;
        loggerManager?.Dispose();
#endif
    }
}