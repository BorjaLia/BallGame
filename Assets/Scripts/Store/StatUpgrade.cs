using UnityEngine;

public enum StatType
{
    Lives,
    SpeedResistance,
    IceResistance
}

[CreateAssetMenu(fileName = "NewStatUpgrade", menuName = "Shop/Stat Upgrade Data")]
public class StatUpgradeData : ScriptableObject
{
    [Header("Identity")]
    public string statId;
    public string displayName;
    public StatType statType;
    public GPGSManager.AchivementNames achivementName;

    [Header("Progression")]
    public float baseValue = 0f;

    public float valuePerLevel = 1f;

    public int maxCap = 5;

    [Header("Pricing")]
    public int baseCost = 10;
    public int costPerLevel = 10;

    public int GetCostForLevel(int targetLevel)
    {
        return baseCost + (targetLevel - 1) * costPerLevel;
    }

    public float GetValueForLevel(int level)
    {
        return baseValue + (level * valuePerLevel);
    }
}