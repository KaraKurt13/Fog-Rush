using Assets.Scripts.TileModifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain
{
    public class Tile
    {
        public GroundTypeEnum GroundType { get; }

        public TileModifierBase Modifier { get; set; }

        public Vector2 Center { get; }

        public int X { get; }
        
        public int Y { get; }

        public Tile(int x, int y, GroundTypeEnum type, Vector2 center)
        {
            X = x;
            Y = y;
            GroundType = type;
            Center = center;
        }
    }
}
