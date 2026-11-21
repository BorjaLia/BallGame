using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool isGyroEnabledDebug = false;

    private bool isGyroEnabled = false;
    public bool IsGyroEnabled { get { return isGyroEnabled; } private set { } }


    [SerializeField]
    private TMP_Text showText;

    [SerializeField]
    private InputActionReference rollControlReference;

    private Vector2 inputDir;

    
    public void ActivateGyro()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        isGyroEnabled = true;
        isGyroEnabledDebug = isGyroEnabled;
#endif
    }

    public void DeactivateGyro()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
        isGyroEnabled = false;
        isGyroEnabledDebug = isGyroEnabled;
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
        string testString = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        Vector3 gyroDir = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.value;
        testString += $"Gyro dir inputSystem: {gyroDir}";
        Debug.Log($"Gyro dir inputSystem: {gyroDir}");
#endif

        testString += " \n";

        Vector2 actionDir = rollControlReference.action.ReadValue<Vector2>();
        testString += $"Gyro dir reference: {actionDir}";
        Debug.Log($"Gyro dir reference: {actionDir}");

        showText.text = testString;
    }
}
