
using System.Collections;
using UnityEngine;
using System;

public class AdvertismentManager : MonoBehaviour
{
    public static AdvertismentManager S;
    [SerializeField] private AdsService _adsService;

    [SerializeField] private float _secondsBetweenInterstitial = 180;
    private float _lastShowInterstitial = 0;
    private Action _onRevarded;
    private bool _isRevarded = false;

    private void Awake()
    {
       if (S == null) 
            S = this;
        _lastShowInterstitial = Time.time - _secondsBetweenInterstitial;
    }
    public void ShowRevarded(Action action)
    {
#if UNITY_EDITOR
        action.Invoke();
        return;
#endif
        Debug.Log("Show Revarded");
        _onRevarded += action;
        EventAggregator.StopPlaySounds.Invoke();
        _adsService.ShowRevarded();
    }
    public void ShowInterstitial()
    {
#if UNITY_EDITOR
        Debug.Log("Show Interstitial");
        return;
#endif
        if (Time.time - _lastShowInterstitial > _secondsBetweenInterstitial)
        {
            Debug.Log("Show Interstitial");
            _adsService.ShowInterstitial();
            EventAggregator.StopPlaySounds.Invoke();
            _lastShowInterstitial = Time.time;
        }
        else
        {
            Debug.Log("Time to next Interstitial: " + (_secondsBetweenInterstitial - (Time.time - _lastShowInterstitial)));
        }
    }
    public void ContinuePlay()
    {
        EventAggregator.ContinuePlaySounds.Invoke();
        if (_isRevarded)
            _onRevarded?.Invoke();
        _onRevarded = null;
        _isRevarded = false;
    }
    public void Revard()
    {
        _isRevarded = true;
    }
}
