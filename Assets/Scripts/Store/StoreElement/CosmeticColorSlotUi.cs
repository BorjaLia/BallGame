using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticColorSlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image colorSwatchImage;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private GameObject selectedOutline;
    [SerializeField] private Button slotButton;

    private CosmeticColorData colorData;
    private PlayerProgressManager progressManager;

    public void Setup(CosmeticColorData data, PlayerProgressManager manager)
    {
        colorData = data;
        progressManager = manager;

        if (colorSwatchImage != null)
        {
            colorSwatchImage.color = data.color;
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
        bool isSelected = currentSelected != null && currentSelected.colorId == colorData.colorId;

        if (lockedOverlay != null)
        {
            lockedOverlay.SetActive(!isUnlocked);
        }

        if (selectedOutline != null)
        {
            selectedOutline.SetActive(isUnlocked && isSelected);
        }

        if (isUnlocked)
        {
            slotButton.interactable = true;
        }
        else
        {
            if (priceText != null)
            {
                priceText.text = colorData.cost.ToString();
            }

            slotButton.interactable = progressManager.Coins >= colorData.cost;
        }
    }

    private void OnSlotClicked()
    {
        if (progressManager.IsColorUnlocked(colorData))
        {
            progressManager.SelectColor(colorData);
        }
        else
        {
            progressManager.TryUnlockColor(colorData);
        }
    }
}