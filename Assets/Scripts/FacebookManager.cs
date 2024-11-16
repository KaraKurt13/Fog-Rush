using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Facebook
{
    public class FacebookManager : MonoBehaviour
    {
        private readonly List<string> _permissions = new() 
        {
            "public_profile",
            "email",
            "userFriends"
        };

        public void Initialize()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(OnInitCallback);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        private void OnInitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }

            FB.LogInWithReadPermissions(_permissions,OnSuccessLogin);
            Debug.Log("Initialized");
        }

        private void OnSuccessLogin(ILoginResult results)
        {
            if (FB.IsLoggedIn)
            {
                var accessToken = results.AccessToken;
                Debug.Log($"User {accessToken.UserId} logged in!");
            }
        }
    }
}