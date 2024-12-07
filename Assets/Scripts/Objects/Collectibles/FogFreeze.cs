using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class FogFreeze : CollectibleType
    {
        public override CollectibleTypeEnum Type => CollectibleTypeEnum.FogFreeze;

        public override Sprite Sprite { get; set; }

        public float FreezeDuration = 3f;

        public override void OnPickUp(Player player)
        {
            Find.Engine.FogWall.TemporaryDeactivate(FreezeDuration);
        }
    }
}
