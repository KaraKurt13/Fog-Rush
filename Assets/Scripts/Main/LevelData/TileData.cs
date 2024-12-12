using Assets.Scripts.Collectibles;
using Assets.Scripts.Objects;
using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class TileData
    {
        public GroundTypeEnum GroundType;

        public MiscTypeEnum MiscType;

        public TerrainLayerEnum TerrainLayer;

        public CollectibleTypeEnum Collectible;
    }
}