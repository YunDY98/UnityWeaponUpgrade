using UnityEngine;
using UnityEngine.UI;
using R3;
using System.Collections;
public class PopUpUI : MonoBehaviour
{
    [Header("PopUp")]

    [SerializeField]
    GameObject deathUI;

    [SerializeField]
    GameObject levelUP;

    [SerializeField]
    GameObject goldWarning;


    WaitForSeconds wait = new(1f);

    [SerializeField]
    Player player;


    StatsVM viewModel;


    void Start()
    {
        viewModel = GameManager.Instance.statsVM;

        Button penalty = deathUI.GetComponentsInChildren<Button>()[0];
        Button ad = deathUI.GetComponentsInChildren<Button>()[1];


        penalty.onClick.AddListener(() =>
        {
            Penalty();
            Resurrction();

        });

#if UNITY_IOS
        AdMobManager.Instance.Reward += WatchedAD;
        AdMobManager.Instance.Failure += Resurrction;
        AdMobManager.Instance.Failure += Penalty;
        ad.onClick.AddListener(() => AdMobManager.Instance.ShowRewardedAd());
#elif UNITY_ANDROID

        UnityAdsManager.Instance.rewardedAds.Reward += WatchedAD;
        UnityAdsManager.Instance.rewardedAds.Failure += Resurrction;
        UnityAdsManager.Instance.rewardedAds.Failure += Penalty;
        ad.onClick.AddListener(() =>
        {
            UnityAdsManager.Instance.rewardedAds.ShowRewardedAd();
        });
#endif

        viewModel.IsDead.Subscribe(dead => deathUI.SetActive(dead));

        viewModel.Level.Skip(1).Subscribe(level =>
        {
            StartCoroutine(PopUp(levelUP));

        });

        viewModel.GoldWarningEvent += GoldWarning;


    }

    void GoldWarning()
    {
        if (goldWarning.activeSelf == true)
            return;
        StartCoroutine(PopUp(goldWarning));
    }

    IEnumerator PopUp(GameObject ui)
    {
        ui.SetActive(true);
        yield return wait;
        ui.SetActive(false);
    }

    void Penalty()
    {

        viewModel.Exp.Value = 0;


    }

    void WatchedAD()
    {
        Resurrction();
        viewModel.AddGold(100000000000);

    }

    void Resurrction()
    {
        deathUI.SetActive(false);
        player.Init();
        viewModel.CurHP.Value = viewModel.GetStat(StatType.MaxHP).value.Value;
        DataManager.Instance.SaveData();

    }

}
