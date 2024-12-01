using Assets.Scripts.Facebook;
using Assets.Scripts.UI;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuEngine : MonoBehaviour
    {
        public FacebookManager FacebookManager;

        public MainMenuUI MainMenuUI;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            if (FB.IsLoggedIn)
            {
                OnSuccessfullLogin();
            }
            else
            {
                FacebookManager.Initialize();
            }
        }

        public void OnSuccessfullLogin()
        {
            MainMenuUI.OnSuccessfulLogin();
        }

        public void OnLogout()
        {
            MainMenuUI.OnConnectionLost();
        }

        public void LoadSingleGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadMultiplayerGame()
        {

        }
    }
}