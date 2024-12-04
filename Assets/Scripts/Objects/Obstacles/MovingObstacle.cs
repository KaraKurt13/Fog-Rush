using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class MovingObstacle : ObstacleBase
    {
        public Engine Engine;

        private OrientationTypeEnum _direction;

        private Vector3 _directionValue;

        private float _endingY;

        private float _speed;

        public void Activate(OrientationTypeEnum direction, float speed, Tile endingTile)
        {
            _direction = direction;
            _speed = speed;
            var value = direction == OrientationTypeEnum.Up ? 1 : -1;
            _directionValue = new Vector3(0, value, 0);
            _endingY = (endingTile.Center + (_directionValue * 2f)).y;
        }

        private void FixedUpdate()
        {
            var pos = transform.position;
            transform.position = pos + (_directionValue * _speed);
            if (EndingCrossed())
                Destroy(gameObject);
        }

        private bool EndingCrossed()
        {
            if (_direction == OrientationTypeEnum.Up)
                return transform.position.y >= _endingY;
            if (_direction == OrientationTypeEnum.Down)
                return transform.position.y <= _endingY;
            return false;
        }

        public override void OnSpawn()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
                OnPlayerTouch(player);
        }

        public override void OnPlayerTouch(Player player)
        {
            Find.Engine.EndGame(player, GameEndStatus.Lose);
        }
    }
}