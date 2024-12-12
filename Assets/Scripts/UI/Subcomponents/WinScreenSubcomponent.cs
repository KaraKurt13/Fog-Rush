using Assets.Scripts.Facebook;
using Assets.Scripts.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WinScreenSubcomponent : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _resultText;
        [SerializeField] Button _shareButton, _nextLevelButton;

        public void Draw(PlayerStats stats)
        {
            _resultText.text = $"{stats.Coins}/3<sprite name=\"Coin\">, {stats.TimeSpent.ToString("F1")} sec.";
            _shareButton.onClick.AddListener(() => FacebookManager.Instance.ShareGameResults(stats));
            _nextLevelButton.onClick.AddListener(() => LevelManager.LoadNextLevel());
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
    }
}
