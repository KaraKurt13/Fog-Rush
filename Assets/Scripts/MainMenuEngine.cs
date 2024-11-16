using Assets.Scripts.Facebook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuEngine : MonoBehaviour
    {
        public FacebookManager FacebookManager;

        private void Start()
        {
            FacebookManager.Initialize();
        }
    }
}