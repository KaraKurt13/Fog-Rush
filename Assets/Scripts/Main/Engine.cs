using Assets.Scripts.Collectibles;
using Assets.Scripts.Main;
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

    public List<CollectibleThing> Collectibles = new();

    //Temp
    public GameObject LevelPrefab;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Find.Engine = this;
        Find.DataLibrary = new DataLibrary();

        var levelData = LevelPrefab?.GetComponent<LevelPrefab>().ConvertPrefabToData() ?? LevelManager.SelectedLevel.ConvertPrefabToData();
        Terrain = LevelGenerator.GenerateLevel(levelData);
        LevelGenerator.SetupPlayers();

        foreach (var obstacle in ObstacleControllers)
        {
            obstacle.Activate();
        }

        if (levelData.FogIsEnabled)
            FogWall.Activate(levelData.FogSpeed);
    }

    public void RestartLevel()
    {
        foreach (var obstacle in ObstacleControllers)
            obstacle.Reset();

        foreach (var collectible in Collectibles)
            collectible.Reset();

        FogWall.Reset();
        Player.Reset();
        GameMenuUI.Reset();
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
