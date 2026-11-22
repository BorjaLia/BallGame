using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerHandler;

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
