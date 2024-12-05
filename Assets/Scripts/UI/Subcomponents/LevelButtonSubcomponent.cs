using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LevelButtonSubcomponent : MonoBehaviour
    {
        public Button Button;

        public TextMeshProUGUI Number;

        [SerializeField] Image _lockImage;

        public void SetLock(bool isLocked)
        {
            _lockImage.gameObject.SetActive(isLocked);
        }
    }
}