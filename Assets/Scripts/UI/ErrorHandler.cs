using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance;

    [SerializeField] TextMeshProUGUI _name, _description;

    [SerializeField] Button _closeButton;

    public void DrawMessage(string name, string message)
    {
        _name.text = name;
        _description.text = message;
        gameObject.SetActive(true);
    }

    private void Undraw()
    {
        gameObject.SetActive(false);
    }
}
