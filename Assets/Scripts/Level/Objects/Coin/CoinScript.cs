using UnityEngine;

public class CoinScript : InteractableObject
{
    [SerializeField] private int value = 1;

    public int Value => value;

    private void OnEnable()
    {
        onPlayerEnterAction += OnPickup;
    }

    private void OnDisable()
    {
        onPlayerEnterAction -= OnPickup;
    }

    private void OnPickup()
    {
        Debug.Log("Published coin collected event");
        eventBus?.Publish(new OnCoinCollected(value));
        Destroy(gameObject, 0.0f);
    }
}