using Assets.Scripts.Main.LevelData;
using Assets.Scripts.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Main.LevelData
{
    public class LevelPrefab : MonoBehaviour
    {
        public Tilemap Ground, Gap;

        public List<ObstacleData> ObstaclesData = new();

        public bool FogIsEnabled = true;

        public float FogSpeed = 1;

        public int Number;

        public LevelTerrainData ConvertPrefabToData()
        {
            var bounds = Ground.cellBounds;
            var terrainSize = bounds.size;
            var width = terrainSize.x;
            var height = terrainSize.y;
            var data = new LevelTerrainData(width, height);
            var groundTiles = Ground.GetTilesBlock(bounds);
            var gapTiles = Gap.GetTilesBlock(bounds);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var index = i + j * width;
                    var tileData = new TileData();
                    var tile = groundTiles[index];

                    if (tile == null)
                    {
                        tile = gapTiles[index];
                        tileData.TerrainLayer = TerrainLayerEnum.Gap;
                    }
                    else
                    {
                        tileData.TerrainLayer = TerrainLayerEnum.Ground;
                    }

                    var type = (GroundTypeEnum)Enum.Parse(typeof(GroundTypeEnum), tile.name);
                    tileData.GroundType = type;
                    data.TerrainData[i,j] = tileData;
                }
            }
            data.ObstacleControllers = ObstaclesData;
            data.FogIsEnabled = FogIsEnabled;
            data.FogSpeed = FogSpeed;

            return data;
        }
    }
}