using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<ExitWallScript> exits = new List<ExitWallScript>();

    public IReadOnlyList<ExitWallScript> Exits => exits;

    public void SetExits(List<ExitWallScript> newExits)
    {
        exits = newExits;
    }

    public void DisarmAllExits()
    {
        foreach (ExitWallScript exit in exits)
        {
            if (exit != null) exit.Disarm();
        }
    }

    public void LockAllExits()
    {
        foreach (ExitWallScript exit in exits)
        {
            if (exit != null) exit.LockExit();
        }
    }
}