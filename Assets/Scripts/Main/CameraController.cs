using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;

    public Player AssignedPlayer;

    private float _staticY, _minBoundX, _maxBoundX;

    private const float _xBoundOffset = 3f;

    private void Start()
    {
        var terrainData = Find.TerrainData;
        var firstLine = terrainData.GetTileLine(0);
        var lastLine = terrainData.GetTileLine(terrainData.Width - 1);

        var center = (firstLine.First().Center + firstLine.Last().Center) / 2;
        _minBoundX = firstLine.First().Center.x + _xBoundOffset;
        _maxBoundX = lastLine.First().Center.x - _xBoundOffset;
        _staticY = center.y;
    }

    private void Update()
    {
        var playerPos = AssignedPlayer.transform.position;
        var x = Mathf.Clamp(playerPos.x, _minBoundX, _maxBoundX);
        Camera.transform.position = new Vector3(x, _staticY, -1);
    }
}
