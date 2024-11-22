using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TileModifiers
{
    public class FinishModifier : TileModifierBase
    {
        public override TileModifierTypeEnum Type => TileModifierTypeEnum.Finish;

        public override void Apply(Player player)
        {
            Find.Engine.EndGame(player, GameEndStatus.Win);
        }
    }
}