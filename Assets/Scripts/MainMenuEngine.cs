using Assets.Scripts.Facebook;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuEngine : MonoBehaviour
    {
        public FacebookManager FacebookManager;

        public MainMenuUI MainMenuUI;

        private void Start()
        {
            FacebookManager.Initialize();
        }

        public void OnSuccessfullLogin()
        {
            MainMenuUI.OnSuccessfulLogin();
        }

        public void OnLogout()
        {
            MainMenuUI.OnConnectionLost();
        }
    }
}