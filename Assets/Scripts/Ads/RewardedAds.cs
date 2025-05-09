using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
{
    [SerializeField] string iosAdUnitId;
    [SerializeField] string androidAdUnitId = "Rewarded_Android";
    public event Action Reward;
    public event Action Failure;


    string adUnitId;
 

    void Awake()
    {

#if UNITY_ANDROID
    adUnitId = androidAdUnitId;
#elif UNITY_IOS
    adUnitId = iosAdUnitId;      
#endif

      



    }


    public void LoadRewardedlAd()
    {
       
        Advertisement.Load(adUnitId, this);

    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        HUD.mobileDebug.text = "ad Loaded";
        Debug.Log("Rewardedl Ad Loaded");
    }

    public void ShowRewardedlAd()
    {
       
        Advertisement.Show(adUnitId, this);
        LoadRewardedlAd();

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        HUD.mobileDebug.text = "FailedToLoad" + " :" + message;
    }

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        HUD.mobileDebug.text = "ShowFailure" + " :" + message;
        Failure?.Invoke();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        HUD.mobileDebug.text = "Ads show Start";
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        HUD.mobileDebug.text = "Ads show Click";

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            HUD.mobileDebug.text = "Ads Fully Watched";
            Debug.Log("Ads Fully Watched");
            GameManager.Instance.isReward = true;
            Reward?.Invoke();
        }
    }
    #endregion
}
