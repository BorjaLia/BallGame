using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    private PlayerHandler controller;

    private PlayerInputHandler inputHandler;

    private Rigidbody rb;

    void Start()
    {
        if (!controller)
        {
            controller = GetComponent<PlayerHandler>();
        }
        else
        {
            Debug.LogError("No controller found");
        }

        if (!inputHandler)
        {
            if (controller) inputHandler = controller.InputScript;
        }

        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("No rigidbody found");
        }
    }

    private void Update()
    {
        
    }
}