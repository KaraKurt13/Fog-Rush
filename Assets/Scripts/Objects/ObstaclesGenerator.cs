using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    private float _spawnInterval, _timeForSpawn;

    private GameObject _obstaclePrefab;

    private Vector3 _spawnPosition;

    private void Start()
    {
        SetRandomValues();
    }

    private void Update()
    {
        
    }

    private void GenerateObstacle()
    {
        Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
    }

    public void SetRandomValues()
    {
        _spawnInterval = 1f;
        _timeForSpawn = _spawnInterval;
    }
}
