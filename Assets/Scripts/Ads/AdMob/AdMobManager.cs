using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdMobManager : MonoBehaviour
{
    public event Action Reward;
    public event Action Failure;
    // 광고 단위 ID (실제 광고 단위 ID로 교체 필요

    private string _adUnitId = "";

    // 보상형 광고 객체
    private RewardedAd _rewardedAd;

    public static AdMobManager Instance { get; private set; }

    void Awake()
    {
#if UNITY_ANDROID
        Destroy(this);
#elif UNITY_IOS
        _adUnitId = new AdsID().adsMobiOSRewardTestId;
#else
        _adUnitId = ""; // 다른 플랫폼에 대한 처리 추가 가능
#endif

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);



    }




    public void Start()
    {
        // Google Mobile Ads SDK 초기화
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads SDK 초기화 완료");
        });

        // 광고 로드
        LoadRewardedAd();
    }

    // 보상형 광고 로드 함수
    public void LoadRewardedAd()
    {
        // 이전 광고가 남아있으면 제거
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
      
        Debug.Log("보상형 광고 로드 중...");

        // 광고 요청 생성
        AdRequest adRequest = new AdRequest();

        // 광고 요청을 통해 광고 로드
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // 로드 실패 시
            if (error != null || ad == null)
            {
               
                Debug.LogError("보상형 광고 로드 실패: " + error);
                return;
            }

            // 광고가 성공적으로 로드되면 콜백 설정
            _rewardedAd = ad;
            Debug.Log("보상형 광고 로드 성공");


            // 광고가 닫히면 처리
            _rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;
        });
    }

    // 광고가 닫힐 때 처리 (재로딩 등)
    private void HandleAdClosed()
    {
        Failure?.Invoke();
        Debug.Log("광고가 닫혔습니다.");

        // 광고가 닫히면 새로운 광고를 로드
        LoadRewardedAd();
    }

    // 보상형 광고 표시 함수
    public void ShowRewardedAd()
    {
         
        // 광고가 준비되었으면 표시
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
           
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("광고 완료 후 보상 지급");
                Reward?.Invoke();
            });
        }
        else
        {
          
            Debug.Log("광고가 아직 로드되지 않았습니다.");
        }
      
    }
}