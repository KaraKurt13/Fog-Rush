using Assets.Scripts.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI
{
    public class LevelSelectionSubmenu : MonoBehaviour
    {
        [SerializeField] Transform _levelsContainer;

        [SerializeField] GameObject _levelPrefab;

        private LevelManager _levelManager;

        public void Draw()
        {
            var levelsData = _levelManager.LevelsData;
            var levelsPrefabs = _levelManager.LevelPrefabs;
            for (int i = 1; i <= levelsData.Count; i++)
            {
                var prefab = levelsPrefabs[i];
                var levelObject = Instantiate(_levelPrefab, _levelsContainer).GetComponent<LevelButtonSubcomponent>();
                var levelNumber = prefab.Number;
                var isUnlocked = levelsData[levelNumber].IsUnlocked;

                levelObject.Number.text = levelNumber.ToString();
                levelObject.Button.onClick.AddListener(() => _levelManager.LoadLevel(levelNumber));
                levelObject.SetLock(!isUnlocked);
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

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
    }
}