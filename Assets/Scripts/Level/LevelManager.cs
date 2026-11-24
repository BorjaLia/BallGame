using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;

    [SerializeField] private PlayerHandler playerHandler;
    [SerializeField] private float fallDeathY = -5f;
    [SerializeField] private float respawnDelay = 0.5f;

    private SceneLoader loader;
    private EventBus eventBus;

    private Vector3 currentCheckpoint;
    private bool isHandlingDeath = false;
    private bool isLevelEnded = false;


    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterAsMonobehaviour<LevelManager>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Remove<LevelManager>();
    }

    void Start()
    {
        FindScriptReferences();

        if (playerHandler != null) currentCheckpoint = playerHandler.transform.position;

        if (eventBus != null)
        {
            eventBus.Subscribe<OnRoomExit>(HandleRoomExit);
            eventBus.Subscribe<OnWin>(HandleLevelWon);
        }

        debugPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) loader.LoadMainMenu();
        CheckFallDeath();
    }

    private void OnDestroy()
    {
        if (eventBus != null)
        {
            eventBus.Unsubscribe<OnRoomExit>(HandleRoomExit);
            eventBus.Unsubscribe<OnWin>(HandleLevelWon);
        }
    }

    public void ToggleDevMode()
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
    }

    private void HandleRoomExit(OnRoomExit e)
    {
        currentCheckpoint = e.ExitWall.CheckpointPosition;
    }

    private void HandleLevelWon(OnWin e)
    {
        isLevelEnded = true;
        SetPlayerActive(false);//trigger ui
    }

    private void CheckFallDeath()
    {
        if (isHandlingDeath || isLevelEnded || playerHandler == null) return;

        if (playerHandler.transform.position.y < fallDeathY)
        {
            StartCoroutine(HandlePlayerDeathRoutine());
        }
    }

    private IEnumerator HandlePlayerDeathRoutine()
    {
        isHandlingDeath = true;
        playerHandler.LoseLife();

        if (playerHandler.CurrentLives > 0)
        {
            SetPlayerActive(false);
            yield return new WaitForSeconds(respawnDelay);

            RespawnAtCheckpoint();
            SetPlayerActive(true);
            isHandlingDeath = false;
        }
        else
        {
            isLevelEnded = true;
            SetPlayerActive(false);
            eventBus?.Publish(new OnLoss());
        }
    }

    private void RespawnAtCheckpoint()
    {
        if (playerHandler.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        playerHandler.transform.position = currentCheckpoint;
    }

    private void SetPlayerActive(bool active)
    {
        if (playerHandler == null) return;

        playerHandler.enabled = active;

        if (playerHandler.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            if (!active)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.isKinematic = !active;
        }
    }

    private void FindScriptReferences()
    {
        loader = ServiceLocator.Instance.Get<SceneLoader>();

        if (!playerHandler)
        {
            Debug.LogError("No player reference set");
        }
    }
}
