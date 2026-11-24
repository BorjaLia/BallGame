using UnityEngine;
using UnityEngine.InputSystem;

public class PluginInput : MonoBehaviour
{
    private SceneLoader loader;

    private void Start()
    {
        loader = ServiceLocator.Instance.Get<SceneLoader>();
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.Get<LoggerTest>().RefreshLogsUI();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) loader.LoadMainMenu();
    }
}
