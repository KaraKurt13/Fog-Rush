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
        _playerIcon.sprite = PlayerData.Icon;
        _playerName.text = PlayerData.Name;
        _playerID.text = PlayerData.ID;
        _authorizedPanel.SetActive(true);
        _unauthorizedPanel.SetActive(false);
    }

    public void DrawUnauthorizedPanel()
    {
        _authorizedPanel.SetActive(false);
        _unauthorizedPanel.SetActive(true);
    }
}
