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
       
        Debug.Log("Ads Insitalized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
       
        
    }

    void Awake()
    {
        
       
           
        #if UNITY_ANDROID
            gameId = new AdsID().unityAdsAndGameId;
        #elif UNITY_IOS
            Destroy(this);
            //gameId = iosGameId;
        #endif
        

            
        if(!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId,isTesting,this);
        }
        
    }

   
}
