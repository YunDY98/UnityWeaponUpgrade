using UnityEngine;
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


        });
    }


    // 보상형 광고 표시 함수
    public void ShowRewardedAd()
    {


        // 광고가 준비되었으면 표시
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            bool isRewarded = false;
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("광고 끝까지 시청 후 보상 지급");
                isRewarded = true;
                Reward?.Invoke();
            });

            // 광고가 닫혔을 때 보상이 지급되지 않았다면 Failure 호출
            _rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                AudioManager.Instance.PlayBGM(true);
                
                Debug.Log("광고가 닫혔습니다.");

                if (!isRewarded)
                {
                    Debug.Log("광고를 끝까지 보지 않아 보상 지급 실패");
                    Failure?.Invoke();
                }

                LoadRewardedAd(); // 광고 재로드
            };

        }
        else
        {
            Failure?.Invoke();
            Debug.Log("광고가 아직 로드되지 않았습니다.");
            return;
        }
        AudioManager.Instance.PlayBGM(false);

    }
}
