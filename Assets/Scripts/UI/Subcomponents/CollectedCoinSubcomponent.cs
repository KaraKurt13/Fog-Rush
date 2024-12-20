using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class CollectedCoinSubcomponent : MonoBehaviour
    {
        [SerializeField] Image _sprite;

        [SerializeField] Color _uncollectedColor;

        public bool IsCollected;

        public void ChangeState(bool isCollected)
        {
            if (isCollected)
                _sprite.color = Color.white;
            else
                _sprite.color = _uncollectedColor;

            IsCollected = isCollected;
        }
    }
}