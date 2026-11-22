using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerHandler;

    private SceneLoader loader;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterAsMonobehaviour<LevelManager>(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Remove<LevelManager>();
    }

    void Start()
    {
        FindScriptReferences();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) loader.LoadMainMenu();
    }

    private void FindScriptReferences()
    {
        loader = ServiceLocator.Instance.Get<SceneLoader>();

        if (!playerHandler)
        {
            Debug.LogError("No player reference set");
        }
    }
}
