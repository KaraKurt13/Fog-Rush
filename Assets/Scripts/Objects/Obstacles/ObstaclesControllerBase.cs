using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public abstract class ObstaclesControllerBase : MonoBehaviour
    {
        public bool IsActive = false;

        public abstract void Activate();

        public abstract void Deactivate();

        public abstract void Init(TileLine line);

        protected abstract void Tick();
    }
}