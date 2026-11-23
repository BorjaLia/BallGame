using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [Header("Header UI")]
    [SerializeField] private TMP_Text coinsText;

    [Header("Page Switching")]
    [SerializeField] private GameObject statsPagePanel;
    [SerializeField] private GameObject cosmeticsPagePanel;
    [SerializeField] private Button statsTabButton;
    [SerializeField] private Button cosmeticsTabButton;

    [Header("Stats Page")]
    [SerializeField] private Transform statsContainer;
    [SerializeField] private StatUpgradeRowUI statRowPrefab;

    [Header("Cosmetics Page")]
    [SerializeField] private Transform cosmeticsContainer;
    [SerializeField] private CosmeticColorSlotUI colorSlotPrefab;

    [SerializeField] private PlayerProgressManager progressManager;
    private EventBus eventBus;

    private readonly List<StatUpgradeRowUI> spawnedStatRows = new List<StatUpgradeRowUI>();
    private readonly List<CosmeticColorSlotUI> spawnedColorSlots = new List<CosmeticColorSlotUI>();

    private void Start()
    {
        if (progressManager == null) progressManager = ServiceLocator.Instance.Get<PlayerProgressManager>();

        eventBus = ServiceLocator.Instance.Get<EventBus>();

        if (progressManager == null)
        {
            Debug.LogError("[ShopUIController] PlayerProgressManager not found in ServiceLocator!");
            return;
        }

        if (statsTabButton != null)
        {
            statsTabButton.onClick.AddListener(ShowStatsPage);
        }

        if (cosmeticsTabButton != null)
        {
            cosmeticsTabButton.onClick.AddListener(ShowCosmeticsPage);
        }

        if (eventBus != null)
        {
            eventBus.Subscribe<CoinsChangedEvent>(HandleCoinsChanged);
            eventBus.Subscribe<StatChangedEvent>(HandleStatChanged);
            eventBus.Subscribe<CosmeticChangedEvent>(HandleCosmeticChanged);
        }

        BuildShopUI();
        ShowStatsPage();
        RefreshAll();
    }

    private void OnDestroy()
    {
        if (eventBus != null)
        {
            eventBus.Unsubscribe<CoinsChangedEvent>(HandleCoinsChanged);
            eventBus.Unsubscribe<StatChangedEvent>(HandleStatChanged);
            eventBus.Unsubscribe<CosmeticChangedEvent>(HandleCosmeticChanged);
        }
    }

    public void ShowStatsPage()
    {
        if (statsPagePanel != null) statsPagePanel.SetActive(true);
        if (cosmeticsPagePanel != null) cosmeticsPagePanel.SetActive(false);
    }

    public void ShowCosmeticsPage()
    {
        if (statsPagePanel != null) statsPagePanel.SetActive(false);
        if (cosmeticsPagePanel != null) cosmeticsPagePanel.SetActive(true);
    }

    private void BuildShopUI()
    {
        foreach (var statData in progressManager.StatUpgrades)
        {
            if (statData == null || statRowPrefab == null || statsContainer == null) continue;
            StatUpgradeRowUI row = Instantiate(statRowPrefab, statsContainer);
            row.Setup(statData, progressManager);
            spawnedStatRows.Add(row);
        }

        foreach (var colorData in progressManager.CosmeticColors)
        {
            if (colorData == null || colorSlotPrefab == null || cosmeticsContainer == null) continue;
            CosmeticColorSlotUI slot = Instantiate(colorSlotPrefab, cosmeticsContainer);
            slot.Setup(colorData, progressManager);
            spawnedColorSlots.Add(slot);
        }
    }

    private void RefreshAll()
    {
        foreach (var row in spawnedStatRows)
        {
            row.Refresh();
        }

        foreach (var slot in spawnedColorSlots)
        {
            slot.Refresh();
        }

        if(coinsText) coinsText.text = "Coins: " + progressManager.Coins;
    }

    private void HandleCoinsChanged(CoinsChangedEvent e) => RefreshAll();
    private void HandleStatChanged(StatChangedEvent e) => RefreshAll();
    private void HandleCosmeticChanged(CosmeticChangedEvent e) => RefreshAll();
}