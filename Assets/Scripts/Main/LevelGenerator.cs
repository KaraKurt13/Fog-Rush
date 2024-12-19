using Assets.Scripts.Collectibles;
using Assets.Scripts.Main;
using Assets.Scripts.Objects;
using Assets.Scripts.Obstacles;
using Assets.Scripts.Terrain;
using Assets.Scripts.TileModifiers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = Assets.Scripts.Terrain.Tile;

public class LevelGenerator : MonoBehaviour
{
    public Engine Engine;

    public TilemapTerrain Tilemaps;

    private int _mapHeight = 10, _mapWidth = 25;

    private TerrainData _terrainData;

    private DataLibrary _dataLibrary;

    [SerializeField] Transform _obstaclesControllerContainer;
    [SerializeField] GameObject _movingObstaclesController, _flickeringObstaclesController;
    [SerializeField] GameObject _collectiblePrefab;
    [SerializeField] TileBase _fogTile;

    private LevelTerrainData _levelData;

    public TerrainData GenerateLevel(LevelTerrainData data)
    {
        _dataLibrary = Find.DataLibrary;
        _levelData = data;
        GenerateTerrain();
        GenerateObstacles();
        return _terrainData;
    }

    private void GenerateTerrain()
    {
        _mapWidth = _levelData.Width;
        _mapHeight = _levelData.Height;
        _terrainData = new TerrainData(_mapWidth, _mapHeight);
        (List<Vector3Int> vectors, List<TileBase> tiles) groundData = new();
        (List<Vector3Int> vectors, List<TileBase> tiles) gapData = new();
        groundData.vectors = new();
        groundData.tiles = new();
        gapData.vectors = new();
        gapData.tiles = new();

        for (int i = 0; i < _levelData.Width; i++)
        {
            var modifier = i == _mapWidth - 1 ? TileModifierTypeEnum.Finish : TileModifierTypeEnum.None;
            for (int j = 0; j < _levelData.Height; j++)
            {
                var tileData = _levelData.TerrainData[i, j];
                var groundType = tileData.GroundType;
                var tileBase = _dataLibrary.TileBases[groundType];
                var vector = new Vector3Int(i, j);
                var center = Tilemaps.Ground.GetCellCenterWorld(vector);
                var tile = new Tile(i, j, center);
                tile.GroundType = groundType;
                tile.Layer = tileData.TerrainLayer;

                switch (tileData.TerrainLayer)
                {
                    case TerrainLayerEnum.Ground:
                        groundData.vectors.Add(vector);
                        groundData.tiles.Add(tileBase);
                        break;
                    case TerrainLayerEnum.Gap:
                        gapData.vectors.Add(vector);
                        gapData.tiles.Add(tileBase);
                        break;
                }

                if (modifier != TileModifierTypeEnum.None)
                {
                    tile.Modifier = _dataLibrary.TileModifiers[modifier];
                }

                if (tileData.Collectible != CollectibleTypeEnum.None)
                {
                    var collectible = Instantiate(_collectiblePrefab, tile.Center, Quaternion.identity).GetComponent<CollectibleThing>();
                    var type = _dataLibrary.CollectibleTypes[tileData.Collectible];
                    collectible.Init(type);
                    Engine.Collectibles.Add(collectible);
                }

                if (tileData.MiscType != MiscTypeEnum.None)
                {
                    var misc = Find.DataLibrary.MiscTileBases[tileData.MiscType];
                    Tilemaps.Misc.SetTile(vector, misc);
                }
                _terrainData.Tiles[i, j] = tile;
            }
            Find.TerrainData = _terrainData;
        }

        Tilemaps.Ground.SetTiles(groundData.vectors.ToArray(), groundData.tiles.ToArray());
        Tilemaps.Gap.SetTiles(gapData.vectors.ToArray(), gapData.tiles.ToArray());

        InitBackgroundFog();

        foreach (var tile in _terrainData.Tiles)
        {
            tile.InitNeighbours(_terrainData);
        }
    }

    private void InitBackgroundFog()
    {
        var padding = 6;
        var width = _terrainData.Width;
        var height = _terrainData.Height;
        var totalWidth = width + padding * 2;
        var totalHeight = height + padding * 2;
        Vector3Int[] positions = new Vector3Int[totalWidth * totalHeight];
        TileBase[] tiles = new TileBase[totalWidth * totalHeight];
        int index = 0;
        for (int x = -padding; x < width + padding; x++)
        {
            for (int y = -padding; y < height + padding; y++)
            {
                positions[index] = new Vector3Int(x, y, 0);
                tiles[index] = _fogTile;
                index++;
            }
        }
        Tilemaps.BackgroundFog.SetTiles(positions, tiles);
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

    public void SetupPlayers()
    {
        var startLine = _terrainData.GetTileLine(0).ToList();
        var startTile = startLine[_levelData.SpawnY];
        Engine.Player.Init(startTile);
    }
}
