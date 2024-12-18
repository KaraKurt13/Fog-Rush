using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class MovingObstacleData : ObstacleData
    {
        public override ObstacleTypeEnum Type => ObstacleTypeEnum.Moving;

        public OrientationTypeEnum MoveDirection;

        public float MoveSpeedPerSecond = 1;

        public float SpawnInterval = 2;
    }
}