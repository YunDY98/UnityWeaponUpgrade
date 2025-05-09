
using UnityEngine;

public class AdsManager : MonoBehaviour
{
   
    public RewardedAds rewardedAds;

    

    public static AdsManager Instance{get; private set;}

    void Awake()
    {
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

