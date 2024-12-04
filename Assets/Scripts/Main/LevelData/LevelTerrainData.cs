using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public class LevelTerrainData
    {
        public int Width, Height;

        public GroundTypeEnum[,] TerrainData;

        public LevelTerrainData(int width, int height)
        {
            Width = width;
            Height = height;
            TerrainData = new GroundTypeEnum[Width, Height];
        }
    }
}