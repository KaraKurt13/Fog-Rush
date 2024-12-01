using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class FlickeringObstacleController : ObstaclesControllerBase
    {
        [SerializeField] GameObject _flickeringObstaclePrefab;

        private float _flickeringInterval, _timeTillFlicker;

        private List<FlickeringObstacle> _obstacles = new();

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
            var obstaclesCount = Random.Range(6,8);
            var randomTiles = line.Tiles.Where(t => t.IsWalkable()).ToList().Random(obstaclesCount);
            foreach (var tile in randomTiles)
            {
                var spawnPos = tile.Center;
                var obstacle = Instantiate(_flickeringObstaclePrefab, spawnPos, Quaternion.identity).GetComponent<FlickeringObstacle>();
                _obstacles.Add(obstacle);
            }
            _flickeringInterval = 3f;
            _timeTillFlicker = _flickeringInterval;
        }

        protected override void Tick()
        {
            _timeTillFlicker -= Time.deltaTime;
            if (_timeTillFlicker <= 0)
            {
                foreach (var obstacle in _obstacles)
                {
                    obstacle.Flicker();
                }
                _timeTillFlicker = _flickeringInterval;
            }
        }

        private void ToggleObstaclesState()
        {

        }
    }
}