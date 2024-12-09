using Assets.Scripts.Facebook;
using Assets.Scripts.MainMenu;
using Facebook.Unity;
using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthController : MonoBehaviour
{
    public MainMenuEngine Engine;

    private FirebaseAuth _auth;

    public FacebookManager FacebookManager;

    private void Awake()
    {
        _auth = FirebaseAuth.DefaultInstance;
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Failed to initialize facebook!");
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    #region Email/Password Authentication
    public void CreateUserWithEmail(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                FirebaseUser newUser = task.Result.User;
                Debug.Log(newUser.Email);
            }
            else
            {
                Debug.Log("Error");
            }
        });
    }

    public void LoginWithEmail(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => 
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
    #endregion Email/Password Authentication

    #region Facebook Authentication
    public async void LoginWithFacebook()
    {
        try
        {
            AccessToken token = await FacebookManager.AuthorizeUser();
            Credential credential = FacebookAuthProvider.GetCredential(token.TokenString);
            AuthResult result = await _auth.SignInAndRetrieveDataWithCredentialAsync(credential);
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            Engine.MainMenuUI.HideLoginOptions();
            Engine.OnSuccessfullLogin();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during Facebook login: " + ex.Message);
        }
       
    }

    #endregion Facebook Authentication

    #region Anonymous
    public void LoginAnonymously()
    {

    }
    #endregion Anonymous

    public void SignOut()
    {
        Engine.MainMenuUI.OnConnectionLost();
        _auth.SignOut();
    }

    private void OnApplicationQuit()
    {
        SignOut();
    }
}
