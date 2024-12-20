using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class GameInterfaceController : MonoBehaviour
    {
        [SerializeField] GameObject _loseScreen, _menuScreen;

        [SerializeField] WinScreenSubcomponent _winScreen;

        public GamePanelComponent GamePanel;

        public void ShowMenu()
        {
            _menuScreen.SetActive(true);
        }
        
        public void HideMenu()
        {
            _menuScreen.SetActive(false);
        }

        public void ShowWinScreen(PlayerStats stats)
        {
            _winScreen.Draw(stats);
        }

        public void ShowLoseScreen()
        {
            _loseScreen.SetActive(true);
        }

        public void Reset()
        {
            _menuScreen.SetActive(false);
            _winScreen.Hide();
            _loseScreen.SetActive(false);
            GamePanel.Reset();
        }
    }
}