using Firebase.Auth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Main
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelPrefab SelectedLevel;

        public static Dictionary<int, LevelPrefab> LevelPrefabs;

        public static Dictionary<int, LevelData> LevelsData;

        private static int _currentLevelNumber;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void LoadDataFromDatabase()
        {
            LoadLevelsData();
            DatabaseManager.LoadLevelData(FirebaseAuth.DefaultInstance.CurrentUser.UserId, result => 
            {
                LevelsData = result;
            });
        }

        public static void LoadLevel(int level)
        {
            if (!LevelPrefabs.TryGetValue(level, out var prefab))
            {
                Debug.LogError($"Level {level} can't be loaded!");
            }

            _currentLevelNumber = level;
            SelectedLevel = prefab;
            SceneManager.LoadScene(1);
        }

        public static void LoadNextLevel()
        {
            var levelNum = _currentLevelNumber + 1;
            LoadLevel(levelNum);
        }

        private static void LoadLevelsData()
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
    }
}