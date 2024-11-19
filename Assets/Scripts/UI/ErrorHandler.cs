using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance;

    [SerializeField] TextMeshProUGUI _name, _description;

    [SerializeField] GameObject _container;
 
    private void Awake()
    {
        Instance = this;
    }

    public void DrawError(string name, string message)
    {
        _name.text = name;
        _description.text = message;
        _container.SetActive(true);
    }

    public void Undraw()
    {
        _container.SetActive(false);
    }
}
