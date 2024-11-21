using Assets.Scripts.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Assets.Scripts.Terrain.Tile;

public class LevelGenerator : MonoBehaviour
{
    public Engine Engine;

    public Queue<TileLine> Lines = new();

    [SerializeField] Tilemap _groundTilemap;

    private Dictionary<GroundTypeEnum, TileBase> _tileBases = new();

    private int _mapHeight = 10;

    public void Initialize()
    {
        InitTilebases();

        for (int i = 0; i < 10; i++)
        {
            Lines.Enqueue(GenerateLine(i));
        }

        SetupPlayers();
    }

    private TileLine GenerateLine(int x)
    {
        var tiles = new List<Tile>();
        var randomBase = _tileBases.Random();
        var vectors = new Vector3Int[_mapHeight];
        var tileBases = new TileBase[_mapHeight];

        for (int i = 0; i < _mapHeight; i++)
        {
            var groundType = randomBase.Key;
            var center = _groundTilemap.GetCellCenterWorld(new Vector3Int(x, i));
            var tile = new Tile(x, i, groundType, center);

            vectors[i] = new Vector3Int(x, i);
            tileBases[i] = randomBase.Value;
            tiles.Add(tile);
        }

        _groundTilemap.SetTiles(vectors, tileBases);

        return new TileLine(tiles);
    }

    private void SetupPlayers()
    {
        var startLine = Lines.Peek();
        var randomTile = startLine.Tiles.Random();
        Engine.Player.gameObject.transform.position = randomTile.Center;
    }

    private void InitTilebases()
    {
        foreach (GroundTypeEnum type in Enum.GetValues(typeof(GroundTypeEnum)))
        {
            var tileBase = Resources.Load<TileBase>($"TileBases/{type}");

            if (tileBase != null)
                _tileBases.Add(type, tileBase);
        }
    }
}
