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
        Terrain = LevelGenerator.InitTerrain();
        LevelGenerator.SetupPlayers();
    }
}
