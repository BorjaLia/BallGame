using UnityEngine;

public class SpeedTileScript : FloorTile
{
    [SerializeField] private float boostForce = 10f;

    public float BoostForce => boostForce;

    protected override void Start()
    {
        base.Start();
        onPlayerEnterEvent = new SpeedBoostTriggeredEvent(boostForce);
    }
}