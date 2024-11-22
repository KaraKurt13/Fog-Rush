using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.TileModifiers
{
    public abstract class TileModifierBase
    {
        public abstract void Apply(Player player);

        public abstract TileModifierTypeEnum Type { get; }
    }
}
