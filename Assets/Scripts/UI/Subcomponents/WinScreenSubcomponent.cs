using Assets.Scripts.Facebook;
using Assets.Scripts.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI
{
    public class WinScreenSubcomponent : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _resultText;
        [SerializeField] Button _shareButton, _nextLevelButton;

        private LevelManager _levelManager;

        public void Draw(PlayerStats stats)
        {
            var timeSpent = TimeHelper.TicksToSeconds(stats.TimeSpent);
            _resultText.text = $"{stats.Coins}/3<sprite name=\"Coin\">, {timeSpent.ToString("F1")} sec.";
            _shareButton.onClick.AddListener(() => FacebookManager.Instance.ShareGameResults(stats));
            _nextLevelButton.onClick.AddListener(() => _levelManager.LoadNextLevel());
            // draw remaining health
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _resultText.text = string.Empty;
            _shareButton.onClick.RemoveAllListeners();
            _nextLevelButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
    }
}
