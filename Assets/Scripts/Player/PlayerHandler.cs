using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementHandler movementScript;
    [SerializeField]
    private PlayerInputHandler inputScript;


    public PlayerMovementHandler MovementScript { get { return movementScript; } private set { } }
    public PlayerInputHandler InputScript { get { return inputScript; } private set { } }

    void Start()
    {
        FindScriptReferences();
    }

    private void FindScriptReferences()
    {
        if (!movementScript)
        {
            movementScript = GetComponent<PlayerMovementHandler>();
        }
        if (!movementScript)
        {
            Debug.LogError("no movement script attached on player");
        }

        if (!inputScript)
        {
            inputScript = GetComponent<PlayerInputHandler>();
        }
        if (!inputScript)
        {
            Debug.LogError("no movement script attached on player");
        }
    }
}
