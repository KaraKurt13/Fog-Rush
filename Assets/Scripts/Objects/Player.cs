using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Engine Engine;

    public Tile CurrentTile;

    public bool IsMoving { get; private set; } = false;

    public bool MovementEnabled { get; private set; } = true;

    public PlayerStatsTracker StatsTracker;

    private Tile _targetTile, _startTile;

    [SerializeField] Animator _animator;

    public void Move(Vector2Int direction)
    {
        var vertical = direction.y;
        var horizontal = direction.x;
        var newTileVector = new Vector2Int(CurrentTile.X + horizontal, CurrentTile.Y + vertical);
        var tile = Engine.Terrain.GetTile(newTileVector.x, newTileVector.y);
        if (tile == null || !tile.IsWalkable())
            return;

        _targetTile = Engine.Terrain.GetTile(newTileVector.x, newTileVector.y);
        IsMoving = true;

        _animator.SetBool("IsMoving", true);
        _animator.SetFloat("X", horizontal);
        _animator.SetFloat("Y", vertical);
        MoveLerp();
    }

    public void Init(Tile tile)
    {
        _startTile = tile;
        CurrentTile = _startTile;
        transform.position = _startTile.Center;
        StatsTracker.StartTracking();
    }

    public void Reset()
    {
        CurrentTile = _startTile;
        transform.position = _startTile.Center;
        IsMoving = false;
        MovementEnabled = true;
        StatsTracker.Reset();
        _animator.Rebind();
        _animator.Update(0f);
        StopAllCoroutines();
    }

    public void DisableMovement()
    {
        MovementEnabled = false;
    }

    public void EnableMovement()
    {
        MovementEnabled = true;
    }

    public bool CanMove()
    {
        return !IsMoving && MovementEnabled;
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
        _animator.SetBool("IsMoving", false);
    }

    private void OnTileChange()
    {
        CurrentTile.Modifier?.Apply(this);
    }
}
