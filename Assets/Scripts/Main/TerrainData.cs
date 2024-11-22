using Assets.Scripts.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData
{
    public Tile[,] Tiles;

    public int Width, Height;

    public Tile GetTile(int x,int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height) return null;
        return Tiles[x,y];
    }

    public TerrainData(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width,Height];
    }
}
