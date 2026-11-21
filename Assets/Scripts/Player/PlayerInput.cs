using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public enum ControlType
    {
        Gyroscope,
        ActionReference
    }

    [SerializeField]
    private bool isGyroEnabledDebug = false;

    private bool isGyroEnabled = false;
    public bool IsGyroEnabled { get { return isGyroEnabled; } private set { } }


    [SerializeField]
    private TMP_Text showText;

    [SerializeField]
    private InputActionReference rollControlReference;



    private Vector3 inputDir;
    private Vector3 inputDirDelta;
    
    public void ActivateGyro()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            isGyroEnabled = true;
            isGyroEnabledDebug = isGyroEnabled;
            Debug.Log("Gyro activated");
        }
        else
        {
            Debug.LogError("Gyro device not found!");
        }

        if (rollControlReference != null)
        {
            rollControlReference.action.Enable();
        }
        else
        {
            Debug.LogError("Action reference not set!");
        }

#endif
    }

    public void DeactivateGyro()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
            isGyroEnabled = false;
            isGyroEnabledDebug = isGyroEnabled;
            Debug.Log("Gyro deactivated");
        }
        else
        {
            Debug.LogError("Gyro device not found!");
        }

        if (rollControlReference != null)
        {
            rollControlReference.action.Disable();
        }
        else
        {
            Debug.LogError("Action reference not set!");
        }
#endif
    }

    void Start()
    {
        isGyroEnabledDebug = isGyroEnabled;
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

#if UNITY_ANDROID && !UNITY_EDITOR

        inputDirDelta = rollControlReference.action.ReadValue<Vector3>();
        inputDir += inputDirDelta * Time.deltaTime;

#endif

        string debugString = $"Gyro dir inputSystem: {inputDirDelta} << \n" +
                                     $"Gyro Total: {inputDir} << \n";

        showText.text = debugString;
        Debug.Log(debugString);
    }
}
