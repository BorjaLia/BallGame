using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movementScript;
    [SerializeField]
    private PlayerInput inputScript;


    public PlayerMovement MovementScript { get { return movementScript; }private set { } }
    public PlayerInput InputScript { get { return inputScript; }private set { } }

    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
        if (!movementScript)
        {
            Debug.LogError("no movement script attached on player");
        }

        inputScript = GetComponent<PlayerInput>();
        if (!inputScript)
        {
            Debug.LogError("no movement script attached on player");
        }
    }
}
