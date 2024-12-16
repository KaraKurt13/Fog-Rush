using Assets.Scripts.Facebook;
using Assets.Scripts.Main;
using Assets.Scripts.UI;
using Facebook.Unity;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuEngine : MonoBehaviour
    {
        public FacebookManager FacebookManager;

        public FacebookAdsManager FacebookAdsManager;

        public MainMenuUI MainMenuUI;

        public AuthController AuthController;

        private LevelManager _levelManager;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }

        public void OnInit()
        {
            if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            {
                OnSuccessfullLogin();
            }
            else
            {
                MainMenuUI.DisplayLoginOptions();
            }
        }

        public void OnSuccessfullLogin()
        {
            _levelManager.LoadDataFromDatabase();
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

        public void QuitApplication()
        {
            Application.Quit();
        }

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
    }
}