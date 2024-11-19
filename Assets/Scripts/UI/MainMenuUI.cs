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

        public void OnSuccessfulLogin()
        {
            StartButton.interactable = true;
            PlayerPanel.DrawAuthorizedPanel();
        }

        public void OnConnectionLost()
        {
            StartButton.enabled = false;
            PlayerPanel.DrawUnauthorizedPanel();
        }
    }
}