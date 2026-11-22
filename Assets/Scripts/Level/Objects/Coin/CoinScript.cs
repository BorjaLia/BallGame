using UnityEngine;

public class CoinScript : InteractableObject
{
    void OnEnable()
    {
        onPlayerCollisionAction += OnPickup;
    }

    void OnDisable()
    {
        onPlayerCollisionAction -= OnPickup;
    }

    private void OnPickup()
    {
        Debug.Log("Coin has been picked up!");
    }
}