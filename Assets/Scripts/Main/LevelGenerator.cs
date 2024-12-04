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
        //SetupTilemap();
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

       /* for (int i = 0; i < _mapWidth; i++)
        {
            
            var line = GenerateLine(i, modifier);
            Lines.Enqueue(line);
            _terrainData.TileLines[i] = line;
        }
       */
        /*foreach (var line in _terrainData.TileLines)
        {
            line.InitNeigbours(_terrainData);
            foreach (var tile in line.Tiles)
            {
                tile.InitNeighbours(_terrainData);
            }
        }*/
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
                        var flickeringObstacle = Instantiate(_movingObstaclesController, _obstaclesControllerContainer)
                            .GetComponent<FlickeringObstacleController>();
                        flickeringObstacle.Init(data);
                        Engine.ObstacleControllers.Add(flickeringObstacle);
                        break;
                    }
            }
        }
    }

    /*private TileLine GenerateLine(int x, TileModifierTypeEnum modifier = TileModifierTypeEnum.None)
    {
        var tiles = new List<Tile>();
        var basicType = GroundTypeEnum.Grass;

        for (int y = 0; y < _mapHeight; y++)
        {
            
            tile.GroundType = basicType;
            if (modifier != TileModifierTypeEnum.None)
            {
                tile.Modifier = _tileModifiers[modifier];
            }
            tiles.Add(tile);
            _terrainData.Tiles[x, y] = tile;
        }

        var tileLine = new TileLine(tiles, x);
        return tileLine;
    }*/

    private void SetupTilemap()
    {
        var tilesCount = _mapHeight * _mapWidth;
        var vectors = new Vector3Int[tilesCount];
        var tileBases = new TileBase[tilesCount];

        int index = 0;
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                var tile = _terrainData.GetTile(x, y);
                vectors[index] = new Vector3Int(x, y);
                tileBases[index] = _tileBases[tile.GroundType];
                index++;
            }
        }
        Tilemaps.Ground.SetTiles(vectors,tileBases);
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


    /*private void GenerateRivers()
    {
        var randomRiverLines = MathHelper.GetRandomUniqueNumbers(1, _mapWidth - 2, 6).OrderBy(t => t).ToList();
        var lines = new List<TileLine>();
        for (int i = 0; i < randomRiverLines.Count; i++)
        {
            var tileLine = _terrainData.GetTileLine(randomRiverLines[i]);
            tileLine.Type = TileLineTypeEnum.River;
            lines.Add(tileLine);
            foreach (var tile in tileLine.Tiles)
            {
                tile.GroundType = GroundTypeEnum.River;
            }
        }

        GenerateBridges(lines);
    }

    private void GenerateBridges(List<TileLine> rivers)
    {
        HashSet<TileLine> processedLines = new();
        while (rivers.Count > 0)
        {
            var currentLine = rivers[0];
            Queue<TileLine> queue = new();
            queue.Enqueue(currentLine);
            List<TileLine> connectedLines = new();
            while (queue.Count > 0)
            {
                var line = queue.Dequeue();
                if (processedLines.Contains(line))
                    continue;
                processedLines.Add(line);
                connectedLines.Add(line);

                foreach (var neighbour in line.Neighbours)
                {
                    if (neighbour.Type == TileLineTypeEnum.River && !processedLines.Contains(neighbour))
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }

            if (connectedLines.Count >= 1)
            {
                var randomBridgesCount = UnityEngine.Random.Range(2, 4);
                var randomNums = MathHelper.GetRandomUniqueNumbers(0, _mapHeight - 1, randomBridgesCount);
                foreach (var num in randomNums)
                {
                    foreach (var tileLine in connectedLines)
                    {
                        var tile = tileLine.GetTile(num);
                        tile.GroundType = GroundTypeEnum.Bridge;
                    }
                }
            }

            rivers.RemoveAll(l => processedLines.Contains(l));
        }
    }*/
}
