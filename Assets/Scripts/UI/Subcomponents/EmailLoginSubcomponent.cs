using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class EmailLoginSubcomponent : MonoBehaviour
    {
        public AuthController AuthController;

        [SerializeField] TMP_InputField _email, _password;

        public void Display()
        { 
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(true);
            _email.text = string.Empty;
            _password.text = string.Empty;
        }

        public void Login()
        {
            if (_email.text == string.Empty || _password.text == string.Empty) return;
            AuthController.LoginWithEmail(_email.text, _password.text);
        }
    }
}