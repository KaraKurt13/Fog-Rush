using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelSubcomponent : MonoBehaviour
{
    [SerializeField] GameObject _unauthorizedPanel, _authorizedPanel;

    [SerializeField] Image _playerIcon;
    [SerializeField] TextMeshProUGUI _playerName, _playerID;

    public void DrawAuthorizedPanel()
    {
        //_playerIcon.sprite = PlayerData.Icon;
        _playerName.text = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        _playerID.text = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //_playerIcon.sprite = 
        _authorizedPanel.SetActive(true);
        _unauthorizedPanel.SetActive(false);
    }

    public void DrawUnauthorizedPanel()
    {
        _authorizedPanel.SetActive(false);
        _unauthorizedPanel.SetActive(true);
    }
}
