using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public class FlickeringObstacle : ObstacleBase
    {
        public override void OnPlayerTouch(Player player)
        {
            Find.Engine.EndGame(player, GameEndStatus.Lose);
        }

        public override void OnSpawn()
        {

        }
    }
}