using Assets.Scripts.Main.LevelData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class FlickeringObstaclesController : ObstaclesControllerBase
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

        public override void Init(ObstacleData baseData)
        {
            var data = baseData as FlickeringObstacleData;
            for (int i = 0; i < data.RowNums.Count; i++)
            {
                var row = data.RowNums[i];
                var tile = Find.TerrainData.GetTile(data.LineNumber, row);
                var spawnPos = tile.Center;
                var obstacle = Instantiate(data.ObstaclePrefab, spawnPos, Quaternion.identity).GetComponent<FlickeringObstacle>();
                _obstacles.Add(obstacle);
            }
            _flickeringInterval = data.FlickeringInterval;
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