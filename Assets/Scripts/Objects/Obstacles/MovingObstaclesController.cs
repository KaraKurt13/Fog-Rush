using Assets.Scripts.Main;
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
        private int _spawnInterval, _ticksForSpawn;

        private Vector3 _spawnPosition;
        private float _moveSpeed;
        private OrientationTypeEnum _moveDirection;

        private TileLine _relatedLine;
        private Tile _endingTile;

        [SerializeField] GameObject _obstaclePrefab;

        private void FixedUpdate()
        {
            if (IsActive)
                Tick();
        }

        public override void Init(ObstacleData baseData)
        {
            var data = baseData as MovingObstacleData;
            var direction = data.MoveDirection;
            var moveDirection = direction == OrientationTypeEnum.Up ? 1 : -1;
            var tileLine = Find.TerrainData.GetTileLine(data.LineNumber);
            var firstTile = tileLine.First();
            var lastTile = tileLine.Last();
            var startTile = direction == OrientationTypeEnum.Up ? firstTile : lastTile;
            var vector = new Vector3(0, moveDirection, 0);

            _moveDirection = direction;
            _moveSpeed = data.MoveSpeedPerSecond / TimeHelper.TicksPerSecond;
            _endingTile = direction == OrientationTypeEnum.Up ? lastTile : firstTile;
            _spawnPosition = startTile.Center - vector * 2;
            _spawnInterval = TimeHelper.SecondsToTicks(data.SpawnInterval);
            _ticksForSpawn = _spawnInterval;
            _obstaclePrefab = data.ObstaclePrefab;
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

        public override void Reset()
        {

        }

        protected override void Tick()
        {
            _ticksForSpawn--;
            if (_ticksForSpawn <= 0)
            {
                GenerateObstacle();
                _ticksForSpawn = _spawnInterval;
            }
        }

        private void GenerateObstacle()
        {
            var obstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity).GetComponent<MovingObstacle>();
            obstacle.Activate(_moveDirection, _moveSpeed, _endingTile);
        }
    }
}
