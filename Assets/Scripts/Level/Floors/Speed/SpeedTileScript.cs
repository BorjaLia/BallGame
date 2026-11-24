using UnityEngine;

public class SpeedTileScript : FloorTile
{
    [SerializeField] private float boostForce = 10f;

    public float BoostForce => boostForce;

    private void OnEnable()
    {
        onPlayerEnterAction += OnPlayerEnter;
    }

    private void OnDisable()
    {
        onPlayerEnterAction -= OnPlayerEnter;
    }

    private void OnPlayerEnter()
    {
        eventBus?.Publish(new OnSpeedBoost());
    }
}