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
    // adUnitId = iosAdUnitId;  
    Destroy(this);
#endif

    }


    public void LoadRewardedlAd()
    {

        Advertisement.Load(adUnitId, this);

    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
    
        Debug.Log("Rewardedl Ad Loaded");
        
    }

    public void ShowRewardedAd()
    {
       
        Advertisement.Show(adUnitId, this);
        LoadRewardedlAd();

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
      
    }

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
       //AudioManager.Instance.PlayBGM(true);
        Failure?.Invoke();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        AudioManager.Instance.PlayBGM(false);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
       
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            AudioManager.Instance.PlayBGM(true);
            Debug.Log("Ads Fully Watched");
            GameManager.Instance.isReward = true;
            Reward?.Invoke();
        }
    }
    #endregion
}
