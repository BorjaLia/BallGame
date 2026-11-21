using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool isGyroEnabled = false;

    [SerializeField]
    private InputActionReference gyroControls;
    
    public bool IsGyroEnabled { get { return isGyroEnabled; } private set { } }
    
    public void ActivateGyro()
    {
        isGyroEnabled = true;
    }

    public void DeactivateGyro()
    {
        isGyroEnabled = false;
    }

    void Start()
    {
    }

    private void OnEnable()
    {
        ActivateGyro();
    }

    private void OnDisable()
    {
        DeactivateGyro();
    }

    void Update()
    {
        
    }
}
