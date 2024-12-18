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

    private Vector3 _startPosition;

    private bool TemporaryDeactivated = true;

    [SerializeField] Tilemap _fogTilemap;
    [SerializeField] TileBase _fogTilebase;

    /// <summary>
    /// </summary>
    /// <param name="movementSpeed">Movement speed per second</param>
    public void Activate(float movementSpeed)
    {
        _movementSpeed = movementSpeed;
        _movementPerTick = movementSpeed / TimeHelper.TicksPerSecond;
        _currentX = _fogTilemap.WorldToCell(transform.position).x;
        _isActive = true;

        var yScale = Find.TerrainData.Height / 3f;
        var line = Find.TerrainData.GetTileLine(0);
        var center = (line.First().Center + line.Last().Center) / 2;

        _startPosition = new Vector3(-3, center.y, 0);
        transform.position = _startPosition;
        transform.localScale = new Vector3(1, yScale, 1);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void TemporaryDeactivate(float time)
    {
        TicksTillActivation = TimeHelper.SecondsToTicks(time);
        TemporaryDeactivated = true;
    }

    public void Reset()
    {
        transform.position = _startPosition;
        _isActive = true;
        TemporaryDeactivated = false;
        _fogTilemap.ClearAllTiles();
    }

    private void FixedUpdate()
    {
        if (_isActive && !TemporaryDeactivated)
            Move();
        if (TemporaryDeactivated)
            DeactivationTick();

    }

    private void Move()
    {
        transform.position += new Vector3(_movementPerTick, 0, 0);
        LineCrossCheck();
    }

    private int TicksTillActivation = 0;

    private void DeactivationTick()
    {
        TicksTillActivation--;
        if (TicksTillActivation <= 0)
            Reactivate();
    }

    private void Reactivate()
    {
        TemporaryDeactivated = false;
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
        var lineTiles = Engine.Terrain.GetTileLine(_currentX);
        if (lineTiles.Count() == 0) return;

        var vectors = lineTiles.Select(t => new Vector3Int(t.X, t.Y, 0)).ToArray();
        var tileBases = Enumerable.Repeat(_fogTilebase, vectors.Count()).ToArray();
        _fogTilemap.SetTiles(vectors, tileBases);
    }
}
