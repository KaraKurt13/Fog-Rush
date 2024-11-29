using Assets.Scripts.Obstacles;
using Assets.Scripts.Terrain;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    private float _spawnInterval, _timeForSpawn;

    private Vector3 _spawnPosition;
    private Vector3 _moveDirection;

    private TileLine _relatedLine;
    private Tile _endingTile;

    private bool _isActive = false;

    [SerializeField] GameObject _obstaclePrefab;

    private void Update()
    {
        if (_isActive)
            Tick();
    }

    public void Init(TileLine relatedLine)
    {
        _relatedLine = relatedLine;
        SetRandomValues();
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
        // delete all obstacles
    }

    private void GenerateObstacle()
    {
        var obstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity).GetComponent<MovingObstacle>();
        obstacle.Activate(_moveDirection, 0.01f, _endingTile);
    }

    private void Tick()
    {
        _timeForSpawn -= Time.deltaTime;
        if (_timeForSpawn <= 0)
        {
            GenerateObstacle();
            _timeForSpawn = _spawnInterval;
        }
    }

    public void SetRandomValues()
    {
        var direction = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        _moveDirection = new Vector3(0, direction, 0);
        var firstTile = _relatedLine.Tiles.First();
        var lastTile = _relatedLine.Tiles.Last();
        var startTile = direction == 1 ? firstTile : lastTile ;

        _endingTile = direction == 1 ? lastTile : firstTile;
        _spawnPosition = startTile.Center - _moveDirection * 2;
        _spawnInterval = Random.Range(2f, 4f);
        _timeForSpawn = _spawnInterval;
    }
}
