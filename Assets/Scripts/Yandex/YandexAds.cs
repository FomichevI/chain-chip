using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexAds : AdsService
{
    [DllImport("__Internal")]
    private static extern void ShowInterstitialExtern();
    [DllImport("__Internal")]
    private static extern void ShowRevardedExtern();


    public override void ShowRevarded()
    {
        ShowRevardedExtern();
    }
    public override void ShowInterstitial()
    {
        ShowInterstitialExtern();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }
    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }
    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
        AudioListener.volume = silence ? 0 : 1;
        Time.timeScale = silence ? 0 : 1;
    }
}
