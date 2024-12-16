using Firebase.Auth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Main
{
    public class LevelManager : IDisposable
    {
        public LevelPrefab SelectedLevel;

        public Dictionary<int, LevelPrefab> LevelPrefabs;

        public Dictionary<int, LevelData> LevelsData;

        private int _currentLevelNumber;

        private DatabaseManager _databaseManager;

        public void LoadDataFromDatabase()
        {
            LoadLevelsData();
            _databaseManager.LoadLevelData(result => 
            {
                LevelsData = result;
            });
        }

        public void LoadLevel(int level)
        {
            if (!LevelPrefabs.TryGetValue(level, out var prefab))
            {
                Debug.LogError($"Level {level} can't be loaded!");
            }

            _currentLevelNumber = level;
            SelectedLevel = prefab;
            SceneManager.LoadScene(1);
        }

        public void LoadNextLevel()
        {
            var levelNum = _currentLevelNumber + 1;
            LoadLevel(levelNum);
        }

        private void LoadLevelsData()
        {
            LevelPrefabs = new();
            var levels = Resources.LoadAll<GameObject>("LevelPrefabs/");
            foreach (var levelObject in levels)
            {
                if (!levelObject.TryGetComponent<LevelPrefab>(out var levelPrefab))
                    continue;
                LevelPrefabs.Add(levelPrefab.Number, levelPrefab);
            }
        }

        [Inject]
        public void Construct(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public void Dispose()
        {
            SelectedLevel = null;
            LevelPrefabs.Clear();
            LevelsData.Clear();
        }
    }
}