using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;

    public Player AssignedPlayer;

    private float _staticY = 5;

    private void Update()
    {
        var playerPos = AssignedPlayer.transform.position;
        var x = Mathf.Clamp(playerPos.x, 9, 10000);
        Camera.transform.position = new Vector3(x, _staticY, -1);
    }
}
