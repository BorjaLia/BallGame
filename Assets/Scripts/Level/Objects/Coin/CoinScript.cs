using UnityEngine;

public class CoinScript : InteractableObject
{
    [SerializeField] private int value = 1;

    public int Value => value;

    private void OnEnable()
    {
        onPlayerCollisionAction += OnPickup;
        onPlayerCollisionEvent = new OnCoinCollected(value);
    }

    private void OnDisable()
    {
        onPlayerCollisionAction -= OnPickup;
    }

    private void OnPickup()
    {
        if (col != null)
            col.enabled = false;

        Debug.Log($"Coin picked up (+{value})!");
        Destroy(gameObject, 1.0f);
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        onPlayerCollisionEvent = new OnCoinCollected(value);
    }
}