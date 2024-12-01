using Assets.Scripts.Obstacles;
using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class MovingObstaclesController : ObstaclesControllerBase
    {
        private float _spawnInterval, _timeForSpawn;

        private Vector3 _spawnPosition;
        private Vector3 _moveDirection;

        private TileLine _relatedLine;
        private Tile _endingTile;

        [SerializeField] GameObject _obstaclePrefab;

        private void Update()
        {
            if (IsActive)
                Tick();
        }

        public override void Init(TileLine relatedLine)
        {
            _relatedLine = relatedLine;
            SetRandomValues();
        }

        public override void Activate()
        {
            IsActive = true;
        }

        public override void Deactivate()
        {
            IsActive = false;
            // Delete all obstacles
        }

        protected override void Tick()
        {
            _timeForSpawn -= Time.deltaTime;
            if (_timeForSpawn <= 0)
            {
                GenerateObstacle();
                _timeForSpawn = _spawnInterval;
            }
        }

        private void GenerateObstacle()
        {
            var obstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity).GetComponent<MovingObstacle>();
            obstacle.Activate(_moveDirection, 0.01f, _endingTile);
        }

        private void SetRandomValues()
        {
            var direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
            _moveDirection = new Vector3(0, direction, 0);
            var firstTile = _relatedLine.Tiles.First();
            var lastTile = _relatedLine.Tiles.Last();
            var startTile = direction == 1 ? firstTile : lastTile;

            _endingTile = direction == 1 ? lastTile : firstTile;
            _spawnPosition = startTile.Center - _moveDirection * 2;
            _spawnInterval = Random.Range(2f, 4f);
            _timeForSpawn = _spawnInterval;
        }
    }
}
