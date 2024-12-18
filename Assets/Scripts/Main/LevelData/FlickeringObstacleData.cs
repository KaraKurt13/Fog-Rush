using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class FlickeringObstacleData : ObstacleData
    {
        public override ObstacleTypeEnum Type => ObstacleTypeEnum.Flickering;

        public float FlickeringInterval = 2;

        public List<int> RowNums;
    }
}