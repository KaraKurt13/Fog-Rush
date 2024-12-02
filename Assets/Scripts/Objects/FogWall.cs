using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogWall : MonoBehaviour
{
    public Engine Engine;

    private bool _isActive = false;

    private float _movementSpeed, _movementPerTick;

    private int _currentX;

    private const float _xOffset = 0.5f;

    [SerializeField] Tilemap _fogTilemap;
    [SerializeField] TileBase _fogTilebase;

    /// <summary>
    /// </summary>
    /// <param name="movementSpeed">Movement speed per second</param>
    public void Activate(float movementSpeed)
    {
        _movementSpeed = movementSpeed;
        _movementPerTick = movementSpeed / 50;
        _currentX = _fogTilemap.WorldToCell(transform.position).x;
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void FixedUpdate()
    {
        if (_isActive)
            Move();
    }

    private void Move()
    {
        transform.position += new Vector3(_movementPerTick, 0, 0);
        LineCrossCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var player))
        {
            Engine.EndGame(player, GameEndStatus.Lose);
        }
    }

    private void LineCrossCheck()
    {
        var pos = transform.position;
        var shiftedPos = new Vector3(pos.x - _xOffset, pos.y, 0);
        var x = _fogTilemap.WorldToCell(shiftedPos).x;
        if (x != _currentX)
        {
            _currentX = x;
            OnLineCross();
        }
    }

    private void OnLineCross()
    {
        var line = Engine.Terrain.GetTileLine(_currentX);
        if (line == null) return;

        var vectors = line.Tiles.Select(t => new Vector3Int(t.X, t.Y, 0)).ToArray();
        var tileBases = Enumerable.Repeat(_fogTilebase, vectors.Count()).ToArray();
        _fogTilemap.SetTiles(vectors, tileBases);
    }
}
