using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public LevelGenerator LevelGenerator;

    private float _speedModifier = 1f;

    void Update()
    {
        foreach (var platform in LevelGenerator.Platforms.ToList())
        {
            var pos = platform.Transform.position;
            pos.x -= 0.04f * _speedModifier;
            platform.Transform.position = pos;
            if (pos.x < -15)
            {
                LevelGenerator.DestroyPlatform(platform);
            }
        }

        LevelGenerator.UpdatePlatforms();
    }

    private void Start()
    {
        LevelGenerator.InitGeneration();
    }
}
