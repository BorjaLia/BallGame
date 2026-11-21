using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movementScript;
    [SerializeField]
    private PlayerInput inputScript;


    public PlayerMovement MovementScript { get { return movementScript; } private set { } }
    public PlayerInput InputScript { get { return inputScript; } private set { } }

    void Start()
    {
        FindScriptReferences();
    }

    private void FindScriptReferences()
    {
        if (!movementScript)
        {
            movementScript = GetComponent<PlayerMovement>();
        }
        if (!movementScript)
        {
            Debug.LogError("no movement script attached on player");
        }

        if (!inputScript)
        {
            inputScript = GetComponent<PlayerInput>();
        }
        if (!inputScript)
        {
            Debug.LogError("no movement script attached on player");
        }
    }
}
