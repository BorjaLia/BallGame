using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeRowUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text statNameText;
    [SerializeField] private TMP_Text levelValueText;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;

    private StatUpgradeData statData;
    private PlayerProgressManager progressManager;

    public void Setup(StatUpgradeData data, PlayerProgressManager manager)
    {
        statData = data;
        progressManager = manager;

        if (statNameText != null)
            statNameText.text = data.displayName;

        decreaseButton.onClick.RemoveAllListeners();
        increaseButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();

        decreaseButton.onClick.AddListener(OnDecreaseClicked);
        increaseButton.onClick.AddListener(OnIncreaseClicked);
        buyButton.onClick.AddListener(OnBuyClicked);

        Refresh();
    }

    public void Refresh()
    {
        if (statData == null || progressManager == null) return;

        int selected = progressManager.GetSelectedStatLevel(statData);
        int unlocked = progressManager.GetUnlockedStatLevel(statData);

        if (levelValueText != null)
        {
            if (statData.statType == StatType.Lives)
            {
                int currentLives = Mathf.RoundToInt(statData.GetValueForLevel(selected));
                int maxUnlockedLives = Mathf.RoundToInt(statData.GetValueForLevel(unlocked));
                levelValueText.text = $"{currentLives} / {maxUnlockedLives}";
            }
            else
            {
                levelValueText.text = $"Lv {selected} / {unlocked}";
            }
        }

        decreaseButton.interactable = selected > 0;
        increaseButton.interactable = selected < unlocked;

        bool isMaxed = unlocked >= statData.maxCap;
        if (isMaxed)
        {
            buyButton.interactable = false;
            if (buyButtonText != null) buyButtonText.text = "-";
        }
        else
        {
            int nextCost = statData.GetCostForLevel(unlocked + 1);
            buyButton.interactable = progressManager.Coins >= nextCost;
            if (buyButtonText != null) buyButtonText.text = $"{nextCost}";
        }
    }

    private void OnDecreaseClicked()
    {
        int current = progressManager.GetSelectedStatLevel(statData);
        progressManager.SetSelectedStatLevel(statData, current - 1);
    }

    private void OnIncreaseClicked()
    {
        int current = progressManager.GetSelectedStatLevel(statData);
        progressManager.SetSelectedStatLevel(statData, current + 1);
    }

    private void OnBuyClicked()
    {
        progressManager.TryUnlockNextStatLevel(statData);
    }
}