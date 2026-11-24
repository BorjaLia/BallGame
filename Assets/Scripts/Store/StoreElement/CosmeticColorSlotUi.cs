using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticColorSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button slotButton;

    private CosmeticColorData colorData;
    private PlayerProgressManager progressManager;

    public void Setup(CosmeticColorData data, PlayerProgressManager manager)
    {
        colorData = data;
        progressManager = manager;

        if (nameText != null)
        {
            nameText.text = data.displayName;
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnSlotClicked);

        Refresh();
    }

    public void Refresh()
    {
        if (colorData == null || progressManager == null) return;

        bool isUnlocked = progressManager.IsColorUnlocked(colorData);
        CosmeticColorData currentSelected = progressManager.GetSelectedColorData();
        bool isEquipped = currentSelected != null && currentSelected.colorId == colorData.colorId;

        if (!isUnlocked)
        {
            if (priceText != null)
            {
                priceText.text = colorData.cost.ToString();
            }

            slotButton.interactable = progressManager.Coins >= colorData.cost;
        }
        else if (isEquipped)
        {
            if (priceText != null)
            {
                priceText.text = "Equipped";
            }

            slotButton.interactable = false;
        }
        else
        {
            if (priceText != null)
            {
                priceText.text = "Equip";
            }

            slotButton.interactable = true;
        }
    }

    private void OnSlotClicked()
    {
        if (colorData == null || progressManager == null) return;

        if (!progressManager.IsColorUnlocked(colorData))
        {
            progressManager.TryUnlockColor(colorData);
            return;
        }

        CosmeticColorData currentSelected = progressManager.GetSelectedColorData();
        bool isEquipped = currentSelected != null && currentSelected.colorId == colorData.colorId;

        if (isEquipped) return;

        progressManager.SelectColor(colorData);
    }
}