using Assets.Scripts.Terrain;
using Assets.Scripts.TileModifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Assets.Scripts.Terrain.Tile;

public class LevelGenerator : MonoBehaviour
{
    public Engine Engine;

    public Queue<TileLine> Lines = new();

    [SerializeField] Tilemap _groundTilemap;

    private Dictionary<GroundTypeEnum, TileBase> _tileBases = new();

    private Dictionary<TileModifierTypeEnum, TileModifierBase> _tileModifiers = new();

    private int _mapHeight = 10, _mapWidth = 15;

    private TerrainData _terrainData;

    public TerrainData InitTerrain()
    {
        InitData();

        _terrainData = new TerrainData(_mapWidth, _mapHeight);
        for (int i = 0; i < _mapWidth; i++)
        {
            var modifier = i == _mapWidth - 1 ? TileModifierTypeEnum.Finish : TileModifierTypeEnum.None;
            Lines.Enqueue(GenerateLine(i, modifier));
        }
        return _terrainData;
    }

    private TileLine GenerateLine(int x, TileModifierTypeEnum modifier = TileModifierTypeEnum.None)
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
            if (modifier != TileModifierTypeEnum.None)
            {
                tile.Modifier = _tileModifiers[modifier];
            }

            vectors[i] = new Vector3Int(x, i);
            tileBases[i] = randomBase.Value;
            tiles.Add(tile);
            _terrainData.Tiles[x, i] = tile;
        }

        _groundTilemap.SetTiles(vectors, tileBases);

        return new TileLine(tiles);
    }

    public void SetupPlayers()
    {
        var startLine = Lines.Peek();
        var randomTile = startLine.Tiles.Random();
        Engine.Player.Init(randomTile);
    }

    private void InitData()
    {
        foreach (GroundTypeEnum type in Enum.GetValues(typeof(GroundTypeEnum)))
        {
            var tileBase = Resources.Load<TileBase>($"TileBases/{type}");

            if (tileBase != null)
                _tileBases.Add(type, tileBase);
        }

        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(TileModifierBase)))
            {
                var instance = (TileModifierBase)Activator.CreateInstance(type);
                _tileModifiers.Add(instance.Type, instance);
            }
        }
    }
}
