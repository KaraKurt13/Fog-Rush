using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class CollectibleThing : MonoBehaviour
    {
        public CollectibleType Type;

        [SerializeField] SpriteRenderer spriteRenderer;

        public void Init(CollectibleType type)
        {
            Type = type;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out var player))
            {
                Type.OnPickUp(player);
            }
        }
    }
}