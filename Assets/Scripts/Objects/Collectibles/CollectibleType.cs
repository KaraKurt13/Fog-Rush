using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public abstract class CollectibleType
    {
        public abstract CollectibleTypeEnum Type { get; }

        public abstract Sprite Sprite { get; set; }

        public abstract void OnPickUp(Player player);
    }
}