using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class FlickeringObstacle : ObstacleBase
    {
        public bool IsActive;

        [SerializeField] SpriteRenderer _spriteRenderer;

        [SerializeField] BoxCollider2D _collider;

        public override void OnPlayerTouch(Player player)
        {
            Find.Engine.EndGame(player, GameEndStatus.Lose);
        }

        public void Flicker()
        {
            IsActive = !IsActive;
            if (IsActive)
                Activate();
            else
                Deactivate();
        }

        private void Deactivate()
        {
            _collider.enabled = false;
            UpdateColor();
        }

        private void Activate()
        {
            _collider.enabled = true;
            UpdateColor();
        }

        private void UpdateColor()
        {
            var color = _spriteRenderer.color;
            color.a = IsActive ? 1f : 0.3f;
            _spriteRenderer.color = color;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
                OnPlayerTouch(player);
        }

        public override void OnSpawn()
        {

        }
    }
}