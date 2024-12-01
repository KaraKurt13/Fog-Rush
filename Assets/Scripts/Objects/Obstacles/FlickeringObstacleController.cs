using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class FlickeringObstacleController : ObstaclesControllerBase
    {
        [SerializeField] GameObject _flickeringObstaclePrefab;

        private void Update()
        {
            if (IsActive)
                Tick();

        }
        public override void Activate()
        {
            IsActive = true;
        }

        public override void Deactivate()
        {
            IsActive = false;
        }

        public override void Init(TileLine line)
        {
            var obstaclesCount = Random.Range(2, 4);
            var randomTiles = line.Tiles.Random(obstaclesCount);
            foreach (var tile in randomTiles)
            {
                var spawnPos = tile.Center;
                var obstacle = Instantiate(_flickeringObstaclePrefab, spawnPos, Quaternion.identity).GetComponent<FlickeringObstacle>();
            }
        }

        protected override void Tick()
        {
        }

        private void ToggleObstaclesState()
        {

        }
    }
}