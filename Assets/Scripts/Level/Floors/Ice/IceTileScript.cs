using UnityEngine;

public class IceTileScript : FloorTile
{
    [SerializeField, Range(0f, 1f)] private float dampingMultiplier = 0.15f;

    [SerializeField, Range(0f, 1f)] private float controlMultiplier = 0.35f;

    protected override void Start()
    {
        base.Start();
        onPlayerEnterEvent = new IceTileEnteredEvent(dampingMultiplier, controlMultiplier);
        onPlayerExitEvent = new IceTileExitedEvent();
    }
}