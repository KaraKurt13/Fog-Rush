using Assets.Scripts.Main.LevelData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Main.LevelData
{
    public class LevelPrefab : MonoBehaviour
    {
        public Tilemap Terrain;

        public List<ObstacleData> ObstaclesData = new();

        public bool FogIsEnabled = true;

        public LevelTerrainData ConvertPrefabToData()
        {
            var bounds = Terrain.cellBounds;
            var terrainSize = bounds.size;
            var width = terrainSize.x;
            var height = terrainSize.y;
            var data = new LevelTerrainData(width, height);
            var tiles = Terrain.GetTilesBlock(bounds);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var index = i + j * width;
                    var tile = tiles[index];
                    if (tile == null) continue;
                    var type = (GroundTypeEnum)Enum.Parse(typeof(GroundTypeEnum), tile.name);

                    data.TerrainData[i,j] = type;
                }
            }

            return data;
        }

        private void GenerateObstacleController(ObstacleData data)
        {

        }
    }
}