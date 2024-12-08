using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleThing : MonoBehaviour
    {
        public CollectibleType Type;

        [SerializeField] SpriteRenderer _spriteRenderer;

        public void Init(CollectibleType type)
        {
            Type = type;
            _spriteRenderer.sprite = type.Sprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                Type.OnPickUp(player);
                Destroy(this.gameObject);
            }
        }
    }
}