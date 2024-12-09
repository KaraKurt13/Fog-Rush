using Assets.Scripts.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LevelSelectionSubmenu : MonoBehaviour
    {
        [SerializeField] Transform _levelsContainer;

        [SerializeField] GameObject _levelPrefab;

        public void Init()
        {

        }

        public void Draw()
        {
            var levelsData = LevelManager.LevelsData;
            var levelsPrefabs = LevelManager.LevelPrefabs.Values;
            foreach (var level in levelsPrefabs)
            {
                var levelObject = Instantiate(_levelPrefab, _levelsContainer).GetComponent<LevelButtonSubcomponent>();
                levelObject.Number.text = level.Number.ToString();
                levelObject.Button.onClick.AddListener(() => LevelManager.LoadLevel(level.Number));
                var isUnlocked = levelsData[level.Number].IsUnlocked; 
                levelObject.SetLock(!isUnlocked);
                Debug.Log($"Level is {isUnlocked}");
            }
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
            ClearLevels();
        }

        private void ClearLevels()
        {
            foreach (Transform level in _levelsContainer.transform)
            {
                Destroy(level.gameObject);
            }
        }
    }
}