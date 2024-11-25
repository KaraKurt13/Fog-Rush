using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class MovingObstacle : ObstacleBase
    {
        private float _moveTime = 5f;

        private Vector3 _targetPos, _spawnPos;

        private void Update()
        {
            
        }

        public override void OnSpawn()
        {
            throw new System.NotImplementedException();
        }
    }
}