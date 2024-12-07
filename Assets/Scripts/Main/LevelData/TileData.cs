using Assets.Scripts.Collectibles;
using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public class TileData
    {
        public GroundTypeEnum GroundType;

        public TerrainLayerEnum TerrainLayer;

        public CollectibleTypeEnum Collectible;
    }
}