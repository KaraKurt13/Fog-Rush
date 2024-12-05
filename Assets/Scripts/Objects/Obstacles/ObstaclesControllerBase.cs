using Assets.Scripts.Main.LevelData;
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

        public abstract void Init(ObstacleData baseData);

        public abstract void Reset();

        protected abstract void Tick();
    }
}