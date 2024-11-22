using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Engine Engine;

    public Tile CurrentTile;

    public bool IsMoving { get; private set; } = false;

    private Tile _targetTile;

    private void Update()
    {
        if (IsMoving)
            MoveLerp();
    }

    public void Move(Tile tile)
    {
        _targetTile = tile;
        IsMoving = true;
    }

    public void Init(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.Center;
    }

    public void DisableMovement()
    {

    }

    private void MoveLerp()
    {
        transform.position = _targetTile.Center;
        CurrentTile = _targetTile;
        OnTileChange();
        ResetMovement();
    }

    private void ResetMovement()
    {
        _targetTile = null;
        IsMoving = false;
    }

    private void OnTileChange()
    {
        CurrentTile.Modifier?.Apply(this);
    }
}
