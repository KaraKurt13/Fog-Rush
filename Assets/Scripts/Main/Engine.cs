using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public LevelGenerator LevelGenerator;

    public Player Player;

    public InputController InputController;

    public TerrainData Terrain;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Find.Engine = this;
        Terrain = LevelGenerator.InitTerrain();
        LevelGenerator.SetupPlayers();
    }

    public void EndGame(Player player, GameEndStatus status)
    {
        if (status == GameEndStatus.Win)
            OnPlayerWin();
        else
            OnPlayerLose();
    }

    private void OnPlayerWin()
    {
        Debug.Log("Player won!");
    }

    private void OnPlayerLose()
    {
        Debug.Log("Player lost!");
    }
}
