using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class Shield : CollectibleType
    {
        public override CollectibleTypeEnum Type => CollectibleTypeEnum.Shield;

        public override Sprite Sprite { get; set; }

        public override void OnPickUp(Player player)
        {
            // Shield mechanics
            Debug.Log("Shield picked up!");
        }
    }
}