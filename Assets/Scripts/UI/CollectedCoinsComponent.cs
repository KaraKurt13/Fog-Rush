using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class CollectedCoinsComponent : ComponentBase
    {
        [SerializeField] CollectedCoinSubcomponent[] _collectedCoins;

        public override void Reset()
        {
            foreach (var coin in _collectedCoins)
                coin.ChangeState(false);
        }

        public void OnCoinCollected()
        {
            _collectedCoins.First(c => !c.IsCollected).ChangeState(true);
        }
    }
}