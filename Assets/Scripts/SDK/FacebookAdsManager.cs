using AudienceNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Facebook
{
    public class FacebookAdsManager : MonoBehaviour
    {
        private AdView _adView;
        private InterstitialAd _interstitialAd;

        [SerializeField] GameObject _adObject;

        private void Start()
        {
            AdSettings.AddTestDevice("ee824333-21fb-423e-8c26-26beb6617faf");
            _adView = new AdView("2311328639234573_2311329229234514", AdSize.BANNER_HEIGHT_50);
            _adView.Register(_adObject);
            _adView.LoadAd();
        }

        public void ShowAd()
        {
            Debug.Log("Simulated ad display");
            if (_adView.IsValid())
            {
#if UNITY_EDITOR
                Debug.Log("Simulated ad display");
#else
                _adView.Show(AdPosition.BOTTOM);
#endif
            }
            else
            {
                Debug.LogError("ERROR DURING ADS SHOW");
            }
        }
    }
}