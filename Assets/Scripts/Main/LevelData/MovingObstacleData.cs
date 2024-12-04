using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public class MovingObstacleData : ObstacleData
    {
        public override ObstacleTypeEnum Type => ObstacleTypeEnum.Moving;

        public OrientationTypeEnum MoveDirection;

        public float MoveSpeed;

        public float SpawnInterval;
    }
}