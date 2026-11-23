using UnityEngine;

[CreateAssetMenu(fileName = "NewCosmeticColor", menuName = "Shop/Cosmetic Color Data")]
public class CosmeticColorData : ScriptableObject
{
    public string colorId;
    public string displayName;
    public Color color = Color.white;
    public int cost = 25;
    public bool unlockedByDefault = false;
}