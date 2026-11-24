using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using UnityEngine;

public class GPGSManager : MonoBehaviour
{
    public enum AchivementNames
    {
        MaxLives,
        MaxFriction,
        MaxWeight
    }

    public static GPGSManager Instance { get; private set; }
    public bool IsSignedIn { get; private set; }

    public readonly Dictionary<AchivementNames, string> Achivements = new Dictionary<AchivementNames, string>();

    void Awake()
    {
        Achivements.Add(AchivementNames.MaxFriction,GPGSIds.achievement_max_friction);
        Achivements.Add(AchivementNames.MaxLives,GPGSIds.achievement_max_lives);
        Achivements.Add(AchivementNames.MaxWeight,GPGSIds.achievement_max_weight);

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

    public void UnlockAchievement(AchivementNames name)
    {
        string value;
        Achivements.TryGetValue(name, out value);
        UnlockAchievement(value);
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