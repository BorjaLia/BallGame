using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Rooms")]
    [SerializeField] private List<Room> rooms = new List<Room>();

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 15f, -5f);
    [SerializeField] private float panDuration = 0.8f;
    [SerializeField] private AnimationCurve panCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Player")]
    [SerializeField] private PlayerHandler playerHandler;

    private int currentRoomIndex = 0;
    private bool isTransitioning = false;
    private EventBus eventBus;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        eventBus = ServiceLocator.Instance.Get<EventBus>();

        if (eventBus != null)
        {
            eventBus.Subscribe<OnRoomExit>(HandleRoomExit);
        }

        if (rooms.Count > 0 && mainCamera != null)
        {
            mainCamera.transform.position = rooms[0].transform.position + cameraOffset;
        }
    }

    private void HandleRoomExit(OnRoomExit e)
    {
        if (isTransitioning) return;

        Room departingRoom = rooms[currentRoomIndex];
        departingRoom.DisarmAllExits();

        if (currentRoomIndex + 1 >= rooms.Count)
        {
            Debug.LogWarning("[RoomManager] No next room assigned in rooms!");
            return;
        }

        isTransitioning = true;
        currentRoomIndex++;
        StartCoroutine(TransitionToRoomRoutine(departingRoom, rooms[currentRoomIndex], e.ExitWall));
    }

    private IEnumerator TransitionToRoomRoutine(Room departingRoom, Room targetRoom, ExitWallScript triggeredExit)
    {
        SetPlayerMovement(false);

        Vector3 camStartPos = mainCamera.transform.position;
        Vector3 camEndPos = targetRoom.transform.position + cameraOffset;

        Vector3 playerStartPos = playerHandler.transform.position;
        Vector3 playerEndPos = triggeredExit.CheckpointPosition;
        playerEndPos.y = playerStartPos.y;

        float elapsed = 0f;
        while (elapsed < panDuration)
        {
            elapsed += Time.deltaTime;
            float t = panCurve.Evaluate(Mathf.Clamp01(elapsed / panDuration));

            mainCamera.transform.position = Vector3.Lerp(camStartPos, camEndPos, t);
            playerHandler.transform.position = Vector3.Lerp(playerStartPos, playerEndPos, t);
            yield return null;
        }

        mainCamera.transform.position = camEndPos;
        playerHandler.transform.position = playerEndPos;

        departingRoom.LockAllExits();

        SetPlayerMovement(true);
        isTransitioning = false;
    }

    private void OnDestroy()
    {
        if (eventBus != null)
        {
            eventBus.Unsubscribe<OnRoomExit>(HandleRoomExit);
        }
    }

    private void SetPlayerMovement(bool enabled)
    {
        if (playerHandler == null) return;

        playerHandler.SetMovementEnabled(enabled);
    }
}