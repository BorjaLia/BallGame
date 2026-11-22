using UnityEngine;

public class CoinScript : InteractableObject
{
    [SerializeField] private int value = 1;

    public int Value { get { return value; } private set { } }

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
        Destroy(gameObject,1.0f);
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }
}