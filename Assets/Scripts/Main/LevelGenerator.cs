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

    public Queue<TileLine> Lines = new();

    public TilemapTerrain Tilemaps;

    private Dictionary<GroundTypeEnum, TileBase> _tileBases = new();

    private Dictionary<TileModifierTypeEnum, TileModifierBase> _tileModifiers = new();

    private int _mapHeight = 10, _mapWidth = 25;

    private TerrainData _terrainData;

    [SerializeField] Transform _obstaclesControllerContainer;
    [SerializeField] GameObject _movingObstaclesController, _flickeringObstaclesController;

    public TerrainData GenerateLevel(LevelTerrainData data)
    {
        InitData();
        for (int i = 0; i < data.Width; i++)
        {
            for (int j = 0; j < data.Height; j++)
            {
                var tile = data.TerrainData[i, j];
                if (tile == GroundTypeEnum.None) continue;
                var tileBase = _tileBases[tile];
                Tilemaps.Ground.SetTile(new Vector3Int(i, j), tileBase);
            }
        }
        //GenerateBasicTerrain();
        //GenerateRivers();
        //GenerateObstacles();
        //SetupTilemap();
        return _terrainData;
    }

    private void GenerateBasicTerrain()
    {
        _terrainData = new TerrainData(_mapWidth, _mapHeight);

        for (int i = 0; i < _mapWidth; i++)
        {
            var modifier = i == _mapWidth - 1 ? TileModifierTypeEnum.Finish : TileModifierTypeEnum.None;
            var line = GenerateLine(i, modifier);
            Lines.Enqueue(line);
            _terrainData.TileLines[i] = line;
        }

        foreach (var line in _terrainData.TileLines)
        {
            line.InitNeigbours(_terrainData);
            foreach (var tile in line.Tiles)
            {
                tile.InitNeighbours(_terrainData);
            }
        }
    }

    private void GenerateRivers()
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
    }

    private void GenerateObstacles()
    {
        var possibleLines = _terrainData.TileLines.Random(6);
        foreach (var line in possibleLines)
        {
            var randomValue = UnityEngine.Random.Range(0, 2);
            var obstaclePrefab = randomValue == 0 ? _movingObstaclesController : _flickeringObstaclesController;
            var obstacle = Instantiate(obstaclePrefab, _obstaclesControllerContainer).GetComponent<ObstaclesControllerBase>();
            obstacle.Init(line);
            Engine.ObstacleControllers.Add(obstacle);
        }
    }

    private void GeneratePowerups()
    {

    }

    private TileLine GenerateLine(int x, TileModifierTypeEnum modifier = TileModifierTypeEnum.None)
    {
        var tiles = new List<Tile>();
        var basicType = GroundTypeEnum.Grass;

        for (int y = 0; y < _mapHeight; y++)
        {
            var center = Tilemaps.Ground.GetCellCenterWorld(new Vector3Int(x, y));
            var tile = new Tile(x, y, center);
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
    }

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
        var startLine = Lines.Peek();
        var randomTile = startLine.Tiles.Random();
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
