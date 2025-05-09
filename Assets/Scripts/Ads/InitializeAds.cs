using UnityEngine;
using UnityEngine.Advertisements;
public class InitializeAds : MonoBehaviour,IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iosGameId;
    [SerializeField] bool isTesting;

    string gameId;

    public void OnInitializationComplete()
    { 
        HUD.mobileDebug.text = "InitComplete";
        Debug.Log("Ads Insitalized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        HUD.mobileDebug.text = message;
        
    }

    void Awake()
    {
        
       
           
        #if UNITY_ANDROID
            gameId = androidGameId;
        #elif UNITY_IOS
            gameId = iosGameId;
        #endif
        

            
        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId,isTesting,this);
        }
        
    }

   
}
