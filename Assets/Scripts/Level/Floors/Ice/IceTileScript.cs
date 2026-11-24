using UnityEngine;

public class IceTileScript : InteractableObject
{
    [SerializeField, Range(0f, 100f)] private float controlMultiplier = 35.0f;

    private void OnEnable()
    {
        onPlayerEnterAction += OnPlayerEnter;
        onPlayerExitAction += OnPlayerExit;
    }

    private void OnDisable()
    {
        onPlayerEnterAction -= OnPlayerEnter;
        onPlayerExitAction -= OnPlayerExit;
    }

    private void OnPlayerEnter()
    {
        eventBus?.Publish(new IceTileEnteredEvent(controlMultiplier));
    }

    private void OnPlayerExit()
    {
        eventBus?.Publish(new IceTileExitedEvent());
    }
}