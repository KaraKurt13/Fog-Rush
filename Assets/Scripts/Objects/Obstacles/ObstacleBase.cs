using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    public abstract class ObstacleBase : MonoBehaviour
    {
        public abstract void OnSpawn();
    }
}