using Assets.Scripts.Facebook;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WinScreenSubcomponent : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _playerTime;
        [SerializeField] Button _shareButton;

        public void Draw(PlayerStats stats)
        {
            _playerTime.text = $"{stats.TimeSpent.ToString("F1")} sec.";
            _shareButton.onClick.AddListener(() => FacebookManager.Instance.ShareGameResults(stats));
            // draw remaining health
            gameObject.SetActive(true);
        }
    }
}
