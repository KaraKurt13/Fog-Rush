using Assets.Scripts.Main.LevelData;
using Assets.Scripts.Obstacles;
using Assets.Scripts.Terrain;
using Assets.Scripts.TileModifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Assets.Scripts.Terrain.Tile;

public class LevelGenerator : MonoBehaviour
{
    public Engine Engine;

    public TilemapTerrain Tilemaps;

    private Dictionary<GroundTypeEnum, TileBase> _tileBases = new();

    private Dictionary<TileModifierTypeEnum, TileModifierBase> _tileModifiers = new();

    private int _mapHeight = 10, _mapWidth = 25;

    private TerrainData _terrainData;

    [SerializeField] Transform _obstaclesControllerContainer;
    [SerializeField] GameObject _movingObstaclesController, _flickeringObstaclesController;

    private LevelTerrainData _levelData;

    public TerrainData GenerateLevel(LevelTerrainData data)
    {
        _levelData = data;
        InitData();
        GenerateBasicTerrain();
        GenerateObstacles();
        SetupTilemap();
        return _terrainData;
    }

    private void GenerateBasicTerrain()
    {
        _mapWidth = _levelData.Width;
        _mapHeight = _levelData.Height;
        _terrainData = new TerrainData(_mapWidth, _mapHeight);

        for (int i = 0; i < _levelData.Width; i++)
        {
            var modifier = i == _mapWidth - 1 ? TileModifierTypeEnum.Finish : TileModifierTypeEnum.None;
            for (int j = 0; j < _levelData.Height; j++)
            {
                var tileData = _levelData.TerrainData[i, j];
                var groundType = tileData.GroundType;
                var tileBase = _tileBases[groundType];
                var center = Tilemaps.Ground.GetCellCenterWorld(new Vector3Int(i, j));
                var tile = new Tile(i, j, center);
                tile.GroundType = groundType;
                tile.Layer = tileData.TerrainLayer;

                // Replace later with SetupTilemaps() with SetTiles instead
                switch (tileData.TerrainLayer)
                {
                    case TerrainLayerEnum.Ground:
                        Tilemaps.Ground.SetTile(new Vector3Int(i, j), tileBase);
                        break;
                    case TerrainLayerEnum.Gap:
                        Tilemaps.Gap.SetTile(new Vector3Int(i, j), tileBase);
                        break;
                }

                if (modifier != TileModifierTypeEnum.None)
                {
                    tile.Modifier = _tileModifiers[modifier];
                }
                _terrainData.Tiles[i, j] = tile;
            }
            Find.TerrainData = _terrainData;
        }

        foreach (var tile in _terrainData.Tiles)
        {
            tile.InitNeighbours(_terrainData);
        }
    }

    private void GenerateObstacles()
    {
        var obstacles = _levelData.ObstacleControllers;
        foreach (var data in obstacles)
        {
            var type = data.Type;
            switch (type)
            {
                case ObstacleTypeEnum.Moving:
                    {
                        var movingObstacle = Instantiate(_movingObstaclesController, _obstaclesControllerContainer)
                            .GetComponent<MovingObstaclesController>();
                        movingObstacle.Init(data);
                        Engine.ObstacleControllers.Add(movingObstacle);
                        break;
                    }
                case ObstacleTypeEnum.Flickering:
                    {
                        var flickeringObstacle = Instantiate(_flickeringObstaclesController, _obstaclesControllerContainer)
                            .GetComponent<FlickeringObstaclesController>();
                        flickeringObstacle.Init(data);
                        Engine.ObstacleControllers.Add(flickeringObstacle);
                        break;
                    }
            }
        }
    }

    private void SetupTilemap()
    {
    }

    public void SetupPlayers()
    {
        var startTiles = _terrainData.GetTileLine(0).ToList();
        var randomTile = startTiles.Random();  
        Engine.Player.Init(randomTile);
    }

    private void InitData()
    {
        foreach (GroundTypeEnum type in Enum.GetValues(typeof(GroundTypeEnum)))
        {
            var tileBase = Resources.Load<TileBase>($"TileBases/{type}");

            if (tileBase == null) continue;

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
