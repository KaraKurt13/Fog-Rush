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

    public ObstaclesGenerator ObstaclesGenerator { get; }

    public TileLine(List<Tile> tiles, int x, ObstaclesGenerator obstaclesGenerator = null)
    {
        Tiles = tiles;
        X = x;
        ObstaclesGenerator = obstaclesGenerator;
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
                Neighbours.Add(neighbour);
            }
        }
    }
}
