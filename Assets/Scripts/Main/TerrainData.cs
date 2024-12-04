using Assets.Scripts.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    public Tile[,] Tiles;

    public TileLine[] TileLines;

    public int Width, Height;

    public Tile GetTile(int x,int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return null;
        return Tiles[x,y];
    }

    public IEnumerable<Tile> GetTileLine(int x)
    {
        if (x < 0 || x >= Width)
        {
            yield break;
        }
        for (int i = 0; i < Height; i++)
        {
            yield return Tiles[x,i];
        }
    }

    public TerrainData(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width,Height];
        TileLines = new TileLine[width];
    }
}
