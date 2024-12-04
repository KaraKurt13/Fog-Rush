using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public class FlickeringObstacleData : ObstacleData
    {
        public override ObstacleTypeEnum Type => ObstacleTypeEnum.Flickering;

        public int FlickeringInterval;

        public List<int> RowNums;
    }
}