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

    public void Move(Tile tile)
    {
        _targetTile = tile;
        IsMoving = true;
        MoveLerp();
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
        StartCoroutine(LerpPosition(CurrentTile.Center, _targetTile.Center, 0.5f));
    }

    private IEnumerator LerpPosition(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
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
