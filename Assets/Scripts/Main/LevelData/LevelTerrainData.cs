using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class LevelTerrainData
    {
        public int Width, Height;

        public TileData[,] TerrainData;

        public List<ObstacleData> ObstacleControllers;

        public bool FogIsEnabled;

        public float FogSpeed;

        public LevelTerrainData(int width, int height)
        {
            Width = width;
            Height = height;
            TerrainData = new TileData[Width, Height];
        }
    }
}