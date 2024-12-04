using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public class MovingObstacleData : ObstacleData
    {
        public override ObstacleTypeEnum Type => ObstacleTypeEnum.Moving;

        public int MoveDirection;

        public float MoveSpeed;

        public float SpawnInterval;
    }
}