
using UnityEngine;

public class UnityAdsManager : MonoBehaviour
{
   
    public RewardedAds rewardedAds;

    

    public static UnityAdsManager Instance{get; private set;}

    void Awake()
    {
        #if UNITY_IOS
            Destroy(this);
        #endif

        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        
        
    }

    void Start()
    {
        rewardedAds.LoadRewardedlAd();
        
    }


}

