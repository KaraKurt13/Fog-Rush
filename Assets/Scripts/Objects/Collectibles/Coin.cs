using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectibles
{
    public class Coin : CollectibleType
    {
        public override CollectibleTypeEnum Type => CollectibleTypeEnum.Coin;

        public override Sprite Sprite { get; set; }

        public override void OnPickUp(Player player)
        {
            player.StatsTracker.IncreaseCoin();
        }
    }
}