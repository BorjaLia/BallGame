using System;
using UnityEngine;

[Serializable]
public struct PlayerStats
{
    public float speedBoostMultiplier;

    [Range(0f, 1f)]
    public float iceGrip;

    public static PlayerStats Default => new PlayerStats
    {
        speedBoostMultiplier = 1f,
        iceGrip = 0f
    };
}