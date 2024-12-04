using Assets.Scripts.Main.LevelData;
using Assets.Scripts.Obstacles;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour
{
    public LevelGenerator LevelGenerator;

    public Player Player;

    public InputController InputController;

    public GameMenuUI GameMenuUI;

    public TerrainData Terrain;

    public FogWall FogWall;

    public List<ObstaclesControllerBase> ObstacleControllers = new();

    //TEMP
    public GameObject Prefab;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Find.Engine = this;
        Terrain = LevelGenerator.GenerateLevel(Prefab.GetComponent<LevelPrefab>().ConvertPrefabToData());
        LevelGenerator.SetupPlayers();
        //FogWall.Activate(1.2f);
        foreach (var obstacle in ObstacleControllers)
        {
            obstacle.Activate();
        }
    }

    public void EndGame(Player player, GameEndStatus status)
    {
        var stats = player.StatsTracker.GetStats();

        FogWall.Deactivate();
        player.DisableMovement();
        player.StatsTracker.StopTracking();

        if (status == GameEndStatus.Win)
            GameMenuUI.ShowWinScreen(stats);
        else
            GameMenuUI.ShowLoseScreen();
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
