using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    [Header("Debug coins")]
    public int extraCoins;

    [Header("Shop Catalog")]
    [SerializeField] private List<StatUpgradeData> statUpgrades = new List<StatUpgradeData>();
    [SerializeField] private List<CosmeticColorData> cosmeticColors = new List<CosmeticColorData>();

    private const string CoinsKey = "Player_Coins";
    private const string SelectedColorKey = "Player_SelectedColor";
    private const string HighestLevelKey = "Player_HighestUnlockedLevel";

    private EventBus eventBus;

    public int Coins { get; private set; }
    public int HighestUnlockedLevel { get; private set; }
    public IReadOnlyList<StatUpgradeData> StatUpgrades => statUpgrades;
    public IReadOnlyList<CosmeticColorData> CosmeticColors => cosmeticColors;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterAsMonobehaviour<PlayerProgressManager>(this);

        LoadProgress();

        Coins += extraCoins;
    }

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
        if (eventBus != null)
        {
            eventBus.Subscribe<OnCoinCollected>(HandleCoinCollected);
        }
        else
        {
            Debug.LogError("[PlayerProgressManager] EventBus not found in ServiceLocator!");
        }
    }

    private void OnDestroy()
    {
        if (eventBus != null)
        {
            eventBus.Unsubscribe<OnCoinCollected>(HandleCoinCollected);
        }

        if (ServiceLocator.Instance.Get<PlayerProgressManager>() == this)
        {
            ServiceLocator.Instance.Remove<PlayerProgressManager>();
        }
    }

    private void HandleCoinCollected(OnCoinCollected e)
    {
        AddCoins(e.Amount);
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();

        eventBus?.Publish(new CoinsChangedEvent(Coins));
    }

    private bool TrySpendCoins(int cost)
    {
        if (Coins < cost) return false;

        Coins -= cost;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();

        eventBus?.Publish(new CoinsChangedEvent(Coins));
        return true;
    }

    public int GetUnlockedStatLevel(StatUpgradeData data)
    {
        if (data == null) return 0;
        return Mathf.Clamp(PlayerPrefs.GetInt(GetStatUnlockedKey(data.statId), 0), 0, data.maxCap);
    }

    public int GetSelectedStatLevel(StatUpgradeData data)
    {
        if (data == null) return 0;
        int unlocked = GetUnlockedStatLevel(data);
        return Mathf.Clamp(PlayerPrefs.GetInt(GetStatSelectedKey(data.statId), 0), 0, unlocked);
    }

    public bool TryUnlockNextStatLevel(StatUpgradeData data)
    {
        if (data == null) return false;

        int currentUnlocked = GetUnlockedStatLevel(data);
        if (currentUnlocked >= data.maxCap) return false;

        int nextLevel = currentUnlocked + 1;
        int cost = data.GetCostForLevel(nextLevel);

        if (!TrySpendCoins(cost)) return false;

        PlayerPrefs.SetInt(GetStatUnlockedKey(data.statId), nextLevel);
        PlayerPrefs.SetInt(GetStatSelectedKey(data.statId), nextLevel);
        PlayerPrefs.Save();

        eventBus?.Publish(new StatChangedEvent(data, nextLevel, nextLevel));
        return true;
    }

    public void SetSelectedStatLevel(StatUpgradeData data, int newLevel)
    {
        if (data == null) return;

        int unlocked = GetUnlockedStatLevel(data);
        int clamped = Mathf.Clamp(newLevel, 0, unlocked);

        PlayerPrefs.SetInt(GetStatSelectedKey(data.statId), clamped);
        PlayerPrefs.Save();

        eventBus?.Publish(new StatChangedEvent(data, clamped, unlocked));
    }

    public PlayerStats GetCalculatedStats()
    {
        PlayerStats result = PlayerStats.Default;

        foreach (var stat in statUpgrades)
        {
            if (stat == null) continue;
            int selected = GetSelectedStatLevel(stat);
            float value = stat.GetValueForLevel(selected);

            switch (stat.statType)
            {
                case StatType.Lives:
                    result.maxLives = Mathf.Max(3, Mathf.RoundToInt(value));
                    break;
                case StatType.SpeedResistance:
                    result.speedBoostMultiplier = Mathf.Max(0f, value);
                    break;
                case StatType.IceResistance:
                    result.iceGrip = Mathf.Clamp01(value);
                    break;
            }
        }

        return result;
    }

    public bool IsColorUnlocked(CosmeticColorData data)
    {
        if (data == null) return false;
        if (data.unlockedByDefault) return true;

        return PlayerPrefs.GetInt(GetColorUnlockedKey(data.colorId), 0) == 1;
    }

    public CosmeticColorData GetSelectedColorData()
    {
        string savedId = PlayerPrefs.GetString(SelectedColorKey, string.Empty);

        foreach (var colorData in cosmeticColors)
        {
            if (colorData != null && colorData.colorId == savedId && IsColorUnlocked(colorData))
                return colorData;
        }

        foreach (var colorData in cosmeticColors)
        {
            if (colorData != null && IsColorUnlocked(colorData))
                return colorData;
        }

        return null;
    }

    public Color GetSelectedColor()
    {
        CosmeticColorData data = GetSelectedColorData();
        return data != null ? data.color : Color.white;
    }

    public bool TryUnlockColor(CosmeticColorData data)
    {
        if (data == null || IsColorUnlocked(data)) return false;
        if (!TrySpendCoins(data.cost)) return false;

        PlayerPrefs.SetInt(GetColorUnlockedKey(data.colorId), 1);
        SelectColor(data);
        return true;
    }

    public void SelectColor(CosmeticColorData data)
    {
        if (data == null || !IsColorUnlocked(data)) return;

        PlayerPrefs.SetString(SelectedColorKey, data.colorId);
        PlayerPrefs.Save();

        eventBus?.Publish(new CosmeticChangedEvent(data));
    }

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex <= HighestUnlockedLevel) return;

        HighestUnlockedLevel = levelIndex;
        PlayerPrefs.SetInt(HighestLevelKey, HighestUnlockedLevel);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
        HighestUnlockedLevel = PlayerPrefs.GetInt(HighestLevelKey, 1);
    }

    private string GetStatUnlockedKey(string id) => $"Stat_{id}_Unlocked";
    private string GetStatSelectedKey(string id) => $"Stat_{id}_Selected";
    private string GetColorUnlockedKey(string id) => $"Color_{id}_Unlocked";

    [ContextMenu("Reset All Progress")]
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        LoadProgress();

        eventBus?.Publish(new CoinsChangedEvent(Coins));
        Debug.Log("[PlayerProgressManager] All progress reset.");
    }
}