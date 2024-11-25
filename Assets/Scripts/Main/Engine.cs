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

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Find.Engine = this;
        Terrain = LevelGenerator.InitTerrain();
        LevelGenerator.SetupPlayers();
    }

    public void EndGame(Player player, GameEndStatus status)
    {
        player.DisableMovement();
        if (status == GameEndStatus.Win)
            OnPlayerWin();
        else
            OnPlayerLose();
    }

    private void OnPlayerWin()
    {
        GameMenuUI.ShowWinScreen();
    }

    private void OnPlayerLose()
    {
        GameMenuUI.ShowLoseScreen();
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
