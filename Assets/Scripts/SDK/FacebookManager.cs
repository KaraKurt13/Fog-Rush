using Assets.Scripts.MainMenu;
using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Assets.Scripts.Facebook
{
    public class FacebookManager : MonoBehaviour
    {
        public MainMenuEngine MenuEngine;

        public static FacebookManager Instance;

        #region Auth
        private readonly List<string> _permissions = new()
        {
            "public_profile",
            "email",
            "user_friends"
        };

        public void Initialize()
        {
            if (!FB.IsInitialized)
            {
                TryInitialize();
            }
            else
            {
                AuthorizeUser();
            }
        }

        public void TryInitialize()
        {
            FB.Init(AuthorizeUser);
        }

        public void AuthorizeUser()
        {
            if (FB.IsInitialized)
            {
                Instance = this;
                FB.ActivateApp();
                FB.LogInWithReadPermissions(_permissions, AuthCallback);
            }
            else
            {
                ErrorHandler.Instance.DrawError("Facebook error", "Can't initialize Facebook!");
            }
        }

        public void Logout()
        {
            FB.LogOut();
            MenuEngine.OnLogout();
        }

        private async void AuthCallback(ILoginResult results)
        {
            if (FB.IsLoggedIn)
            {
                await Task.WhenAll(GetUserData(), GetUserPicture());
                FB.AppRequest(
                    "I need your help!"
                    );
                MenuEngine.OnSuccessfullLogin();
            }
            else
            {
                ErrorHandler.Instance.DrawError("Login Error", "Can't log in Facebook! Try again.");
            }
        }

        private async Task GetUserData()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            FB.API("/me?fields=name,id", HttpMethod.GET, result =>
            {
                if (result.ResultDictionary.TryGetValue("name", out var name))
                {
                    PlayerData.Name = name as string;
                }
                else
                {
                    Debug.LogWarning("Failed to get user name.");
                }

                if (result.ResultDictionary.TryGetValue("id", out var id))
                {
                    PlayerData.ID = id as string;
                }
                else
                {
                    Debug.LogWarning("Failed to get user ID.");
                }
                tcs.SetResult(true);
            });

            await tcs.Task;
        }

        private async Task GetUserPicture()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            FB.API("/me?fields=picture.width(100).height(100)", HttpMethod.GET, result =>
            {
                if (result.Texture != null)
                {
                    var texture = result.Texture;
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    PlayerData.Icon = sprite;
                    Debug.Log("User picture loaded.");
                }
                else
                {
                    Debug.LogWarning("Failed to get user picture.");
                }

                tcs.SetResult(true);
            });

            await tcs.Task;
        }
        #endregion Auth

        public void ShareGameResults(PlayerStats stats)
        {
            FB.FeedShare(
                link: new System.Uri("https://facebook.com"),
                linkName: "My record!",
                linkCaption: "Try and beat me!",
                linkDescription: $"I just finished this level in {stats.TimeSpent} sec!\nWith {stats.RemainingHealth} remaining health!"
                );
        }

        public void ShareApp()
        {
            if (AccessToken.CurrentAccessToken.ExpirationTime < DateTime.Now)
            {
                Debug.LogWarning("Access token has expired!");
                // Display re-login window
            }
            FB.ShareLink(
                new Uri("https://www.facebook.com/"),
                "TestTitle",
                "TestDescription",null,OnShare);
        }

        private void OnShare(IShareResult result)
        {
            if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
            {
                Debug.LogWarning("Canceled");
                return;
            }
            else
            {
                Debug.Log("shared!");
            }
        }
    }
}