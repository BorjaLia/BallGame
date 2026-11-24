using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance { get; private set; }
    public bool IsSignedIn { get; private set; }
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
#endif
    }
    void ProcessAuthentication(SignInStatus status)
    {
        IsSignedIn = status == SignInStatus.Success;
        Debug.Log("GPGS login: " + status);
    }
    public void SignInManually()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    public void UnlockAchievement(string id)
    {
        if (!IsSignedIn) return;
        PlayGamesPlatform.Instance.ReportProgress(id, 100.0, ok =>
        Debug.Log("Achivement " + id + ": " + ok));
    }

    public void IncrementAchievement(string id, int steps)
    {
        if (!IsSignedIn) return;
        PlayGamesPlatform.Instance.IncrementAchievement(id, steps, ok =>
        Debug.Log("Increment " + id + ": " + ok));
    }

    public void ShowAchievements()
    {
        if (IsSignedIn) PlayGamesPlatform.Instance.ShowAchievementsUI();
    }
}