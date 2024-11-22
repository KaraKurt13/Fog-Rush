using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Engine Engine;

    public Tile CurrentTile;

    public bool IsMoving = false;

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

    private void MoveLerp()
    {
        transform.position = _targetTile.Center;
        CurrentTile = _targetTile;
        ResetMovement();
    }

    private void ResetMovement()
    {
        _targetTile = null;
        IsMoving = false;
    }
}
