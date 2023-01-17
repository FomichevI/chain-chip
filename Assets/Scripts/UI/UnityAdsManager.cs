using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsListener
{
    public static UnityAdsManager S;
    private float lastShowInterstitial = -300;

    private void Awake()
    {
        S = this;
        Advertisement.AddListener(this);
        Advertisement.Initialize("4403361", false);
    }

    public void ShowInterstitial()
    {
        if (Time.time - lastShowInterstitial > 180)
            if (Advertisement.IsReady())
            {
                Advertisement.Show("Interstitial_Android");
                lastShowInterstitial = Time.time;
            }
    }
    public void ShowRevarded()
    {
        if (Advertisement.IsReady())
            Advertisement.Show("Rewarded_Android");
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished && placementId == "Rewarded_Android")
        {
            MenuManager.S.isWorkShowNoThanksCor = false;
            SkillsController.S.RaiseSkillFilling(SkillsController.S.maxFillingSkills, MenuManager.S.lastSkillColor);
            MenuManager.S.lastSkillColor = eChipColors.no;
            MenuManager.S.HideAllPanels();
        }
    }
}
