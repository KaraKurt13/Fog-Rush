using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleThing : MonoBehaviour
    {
        public CollectibleType Type;

        [SerializeField] SpriteRenderer _spriteRenderer;
        [SerializeField] BoxCollider2D _collider;

        public void Init(CollectibleType type)
        {
            Type = type;
            _spriteRenderer.sprite = type.Sprite;
        }

        public void Reset()
        {
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                Type.OnPickUp(player);
                _spriteRenderer.enabled = false;
                _collider.enabled = false;
            }
        }
    }
}