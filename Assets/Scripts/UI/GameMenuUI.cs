using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject _winScreen, _loseScreen, _menuScreen;

        public void ShowMenu()
        {
            _menuScreen.SetActive(true);
        }

        public void ShowWinScreen()
        {
            _winScreen.SetActive(true);
        }

        public void ShowLoseScreen()
        {
            _loseScreen.SetActive(true);
        }
    }
}