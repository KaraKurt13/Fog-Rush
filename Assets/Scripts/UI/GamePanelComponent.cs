using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GamePanelComponent : ComponentBase
    {
        public CollectedCoinsComponent CollectedCoinsUI;

        private void Start()
        {
            Reset();
        }

        public override void Reset()
        {
            CollectedCoinsUI.Reset();
        }
    }
}