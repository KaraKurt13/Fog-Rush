using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine
{
    public List<Tile> Tiles { get; }

    public TileLine(List<Tile> tiles)
    {
        Tiles = tiles;
    }
}
