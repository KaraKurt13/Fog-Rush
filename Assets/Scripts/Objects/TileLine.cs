using Assets.Scripts.Obstacles;
using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine
{
    public List<Tile> Tiles { get; }

    public int X { get; }

    public List<TileLine> Neighbours { get; private set; }

    public TileLineTypeEnum Type { get; set; }

    public ObstaclesControllerBase ObstacleController { get; }

    public TileLine(List<Tile> tiles, int x, ObstaclesControllerBase obstacleController = null)
    {
        Tiles = tiles;
        X = x;
        ObstacleController = obstacleController;
    }

    public Tile GetTile(int y)
    {
        return Tiles[y];
    }

    public void InitNeigbours(TerrainData terrainData)
    {
        Neighbours = new();
        int[] horizontalOffsets = { -1, 1 };

        for (int i = 0; i < horizontalOffsets.Length; i++)
        {
            int newCol = X + horizontalOffsets[i];
            var neighbour = terrainData.GetTileLine(newCol);

            if (neighbour != null)
            {
                //Neighbours.Add(neighbour);
            }
        }
    }

    public void Deactivate()
    {
        ObstacleController.Deactivate();
    }
}
