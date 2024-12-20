using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class ComponentBase : MonoBehaviour
    {
        public virtual void Draw()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public abstract void Reset();
    }
}