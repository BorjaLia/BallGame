using UnityEngine;

public class WinWallScript : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;
    private EventBus eventBus;

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        if (eventBus != null)
        {
            eventBus.Publish(new OnWin());
        }
        else
        {
            Debug.LogError("[WinWallScript] EventBus not found in ServiceLocator.");
        }
    }
}