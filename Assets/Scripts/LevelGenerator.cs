using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> _platformPrefabs;

    [SerializeField] GameObject _startPlatform;

    [SerializeField] Transform _platformsContainer;

    public List<PlatformObject> Platforms = new();

    private int _maxPlatforms = 6;

    public void UpdatePlatforms()
    {
        if (Platforms.Count < _maxPlatforms)
        {
            GeneratePlatform();
        }
    }

    public void DestroyPlatform(PlatformObject platform)
    {
        Platforms.Remove(platform);
        platform.Destroy();
    }

    public void GeneratePlatform()
    {
        var position = Platforms.Last().Transform.position;
        position.x += 15;
        var platform = Instantiate(_platformPrefabs.Random(), position, Quaternion.identity, _platformsContainer).GetComponent<PlatformObject>();
        Platforms.Add(platform);
    }

    public void InitGeneration()
    {
        var startPlatform = Instantiate(_startPlatform, _platformsContainer).GetComponent<PlatformObject>();
        Platforms.Add(startPlatform);
        Debug.Log(Platforms.Count);
        while (Platforms.Count != _maxPlatforms)
        {
            GeneratePlatform();
        }
    }
}
