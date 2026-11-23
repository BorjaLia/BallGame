using UnityEngine;

public class ExitWallScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject blockingDoorObject;
    [SerializeField] private Transform checkpointPoint = null;

    [SerializeField] private string playerTag = "Player";

    private bool hasTriggered = false;
    private EventBus eventBus;
   
    public Vector3 CheckpointPosition => checkpointPoint != null ? checkpointPoint.position : transform.position;

    private void Start()
    {
        eventBus = ServiceLocator.Instance.Get<EventBus>();

        if (blockingDoorObject != null)
        {
            blockingDoorObject.SetActive(false);
        }

        if(checkpointPoint == null)
        {
            checkpointPoint = transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;

        if (eventBus != null)
        {
            eventBus.Publish(new OnRoomExit(this));
        }
        else
        {
            Debug.LogError("[ExitWallScript] EventBus not found in ServiceLocator.");
        }
    }
    public void Disarm()
    {
        hasTriggered = true;
    }

    public void LockExit()
    {
        if (blockingDoorObject != null)
        {
            blockingDoorObject.SetActive(true);
        }
    }
}