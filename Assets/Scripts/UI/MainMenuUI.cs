using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public Button StartButton;

        public PlayerPanelSubcomponent PlayerPanel;

        [SerializeField] GameObject _gameModeSelectorPanel;

        [SerializeField] GameObject _mainPanel, _loginOptions;

        public void OnSuccessfulLogin()
        {
            StartButton.interactable = true;
            PlayerPanel.DrawAuthorizedPanel();
            
        }

        public void OnConnectionLost()
        {
            StartButton.interactable = false;
            PlayerPanel.DrawUnauthorizedPanel();
            HideGameModeSelector();
        }

        public void DisplayGameModeSelector()
        {
            _gameModeSelectorPanel.SetActive(true);
            _mainPanel.SetActive(false);
        }

        public void HideGameModeSelector()
        {
            _gameModeSelectorPanel.SetActive(false);
            _mainPanel.SetActive(true);
        }

        public void DisplayLoginOptions()
        {
            _loginOptions.SetActive(true);
        }

        public void HideLoginOptions()
        {
            _loginOptions.SetActive(false);
        }
    }
}