using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    private float _spawnInterval, _timeForSpawn;

    private GameObject _obstaclePrefab;

    private Vector3 _spawnPosition;

    private bool _isActive = false;

    private void Update()
    {
        
    }

    public void Activate(TileLine relatedLine)
    {
        SetRandomValues();
    }

    private void GenerateObstacle()
    {
        Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
    }

    public void SetRandomValues()
    {
        _spawnInterval = Random.Range(2f, 4f);
        _timeForSpawn = _spawnInterval;
    }
}
