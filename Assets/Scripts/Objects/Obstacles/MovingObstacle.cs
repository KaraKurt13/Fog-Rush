using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class MovingObstacle : ObstacleBase
    {
        private Vector3 _direction;

        private float _endingY;

        private float _speed;

        public void Activate(Vector3 direction, float speed, Tile endingTile)
        {
            _direction = direction;
            _speed = speed;
            _endingY = (endingTile.Center + (_direction * 2f)).y;
        }

        private void Update()
        {
            var pos = transform.position;
            transform.position = pos + (_direction * _speed);
            if (transform.position.y >= _endingY)
                Destroy(gameObject);
        }

        public override void OnSpawn()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("player touched obstacle");
        }
    }
}