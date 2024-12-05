using Assets.Scripts.Main.LevelData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Main
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelPrefab SelectedLevel;

        public static Dictionary<int, LevelPrefab> LevelPrefabs;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadLevelsData();
        }

        public static void LoadLevel(int level)
        {
            if (!LevelPrefabs.TryGetValue(level, out var prefab))
            {
                Debug.LogError($"Level {level} can't be loaded!");
            }

            SelectedLevel = prefab;
            SceneManager.LoadScene(1);
        }

        private void LoadLevelsData()
        {
            LevelPrefabs = new();
            var levels = Resources.LoadAll<GameObject>("LevelPrefabs/");

            // Later add IsUnlocked set during to loaded data from database
            foreach (var levelObject in levels)
            {
                if (!levelObject.TryGetComponent<LevelPrefab>(out var levelPrefab))
                    continue;
                LevelPrefabs.Add(levelPrefab.Number, levelPrefab);
            }
        }
    }
}