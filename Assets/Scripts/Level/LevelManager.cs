using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerHandler;

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

    private void FindScriptReferences()
    {
        if (!playerHandler)
        {
            Debug.LogError("No player reference set");
        }
    }
}
