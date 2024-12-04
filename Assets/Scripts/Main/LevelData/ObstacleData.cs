using Assets.Scripts.Obstacles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Main.LevelData
{
    public abstract class ObstacleData : MonoBehaviour
    {
        public abstract ObstacleTypeEnum Type { get; }

        public int LineNumber;

        public GameObject ObstaclePrefab;
    }
}