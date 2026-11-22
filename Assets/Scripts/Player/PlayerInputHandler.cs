using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public enum ControlType
    {
        Gyroscope,
        Keyboard,
        Joystick
    }

    [Header("Settings")]
    [SerializeField] private ControlType currentControlType = ControlType.Keyboard;
    [SerializeField] private float resetGyroTime = 1.75f;
    [SerializeField] private float currentResetTime = 0.0f;

    [Header("Input Action References")]
    [SerializeField] private InputActionReference gyroControlReference;
    [SerializeField] private InputActionReference keyboardControlReference;
    [SerializeField] private InputActionReference joystickControlReference;

    [Header("UI References")]
    [SerializeField] private GameObject onScreenJoystickUI;
    [SerializeField] private TMP_Text showText;
    

    private bool isGyroEnabled = false;
    public bool IsGyroEnabled => isGyroEnabled;
    public ControlType CurrentControlType => currentControlType;


    public Vector2 MoveInput { get; private set; }

    private Vector3 gyroDir;
    private Vector3 gyroDirDelta;

    void Start()
    {
        MoveInput = Vector3.zero;
        SetControlType(currentControlType);
    }

    private void OnEnable()
    {
        ActivateGyro();
    }

    private void OnDisable()
    {
        DeactivateGyro();
    }

    public void SetControlTypeTest(int type)
    {
        SetControlType((ControlType)type);
    }

    public void SetControlType(ControlType newType)
    {
        DeactivateAllInputs();
        currentControlType = newType;

        switch (currentControlType)
        {
            case ControlType.Gyroscope:
                ActivateGyro();
                break;
            case ControlType.Keyboard:
                ActivateKeyboard();
                break;
            case ControlType.Joystick:
                ActivateJoystick();
                break;
        }
    }

    public void ActivateGyro()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
            isGyroEnabled = true;
            Debug.Log("Gyro activated");
        }
        else
        {
            Debug.LogError("Gyro device not found!");
        }

        if (gyroControlReference != null)
        {
            gyroControlReference.action.Enable();
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
            Debug.Log("Gyro deactivated");
        }
        else
        {
            Debug.LogError("Gyro device not found!");
        }

        if (gyroControlReference != null)
        {
            gyroControlReference.action.Disable();
        }
        else
        {
            Debug.LogError("Action reference not set!");
        }
#endif
    }

    public void ActivateKeyboard()
    {
        if (keyboardControlReference != null)
        {
            keyboardControlReference.action.Enable();
        }
        else
        {
            Debug.LogError("Keyboard action reference not set!");
        }
    }

    public void DeactivateKeyboard()
    {
        if (keyboardControlReference != null)
        {
            keyboardControlReference.action.Disable();
        }
        else
        {
            Debug.LogError("Keyboard action reference not set!");
        }
    }

    public void ActivateJoystick()
    {
        if (onScreenJoystickUI != null)
        {
            onScreenJoystickUI.SetActive(true);
        }
        else
        {
            Debug.LogError("UI Joystick not set!");
        }

        if (joystickControlReference != null)
        {
            joystickControlReference.action.Enable();
        }
        else
        {
            Debug.LogError("Joystick action reference not set!");
        }
    }

    public void DeactivateJoystick()
    {
        if (onScreenJoystickUI != null)
        {
            onScreenJoystickUI.SetActive(false);
        }
        else
        {
            Debug.LogError("UI Joystick not set!");
        }

        if (joystickControlReference != null)
        {
            joystickControlReference.action.Disable();
        }
        else
        {
            Debug.LogError("Joystick action reference not set!");
        }
    }

    private void DeactivateAllInputs()
    {
        DeactivateGyro();
        DeactivateKeyboard();
        DeactivateJoystick();
        MoveInput = Vector2.zero;
    }


    void CheckGyro()
    {
        if(gyroDirDelta != Vector3.zero)
        {
            currentResetTime = 0.0f;
            return;
        }

        currentResetTime += Time.deltaTime;

        if (currentResetTime <= resetGyroTime)
        {
            gyroDir = Vector3.zero;
        }
    }

    void Update()
    {
        string debugString = $"Mode: {currentControlType}\n";

        switch (currentControlType)
        {
            case ControlType.Gyroscope:
                {
                    CheckGyro();

#if UNITY_ANDROID && !UNITY_EDITOR

                    gyroDirDelta = gyroControlReference.action.ReadValue<Vector3>();
                    gyroDir += new Vector3(gyroDirDelta.y,-gyroDirDelta.x,0.0f) * Time.deltaTime;

                    MoveInput = gyroDir;
#endif

                    debugString += $"Gyro Delta: {gyroDirDelta}\nGyro Total: {gyroDir}\n";
                    break;
                }

            case ControlType.Keyboard:
                {

                    if (keyboardControlReference != null)
                    {
                        MoveInput = keyboardControlReference.action.ReadValue<Vector2>();
                    }
                    break;
                }

            case ControlType.Joystick:
                {

                    if (joystickControlReference != null)
                    {
                        MoveInput = joystickControlReference.action.ReadValue<Vector2>();
                    }
                    break;
                }
        }

        if (MoveInput != Vector2.zero) debugString += $"Move output: {MoveInput}";

        showText.text = debugString;
        //Debug.Log(debugString);
    }
}
