using UnityEngine;

public class SpeedTileScript : InteractableObject
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
        Debug.Log("Published speedBoost event");
        eventBus?.Publish(new OnSpeedBoost(boostForce));
    }
}