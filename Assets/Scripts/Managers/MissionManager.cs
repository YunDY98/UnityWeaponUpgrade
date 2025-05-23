using System.Numerics;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts;
using Unity.Android.Gradle.Manifest;



public class MissionManager : MonoBehaviour, IPointerDownHandler
{
    private static MissionManager _instance;

    public static MissionManager Instance
    {
        get => _instance;
    }

    #region MissionData
    public int missionID = 0;
    public int kill = 0;
    public BigInteger earnedGold = new();

    #endregion MissionData

    ReactiveProperty<BigInteger> curValue = new();
    BigInteger goal = new();
    //int curValue;
    string missionType;
    StatType statType;
    string rewardType;
    BigInteger reward = new();

    bool isClear = false;





    [SerializeField] TextMeshProUGUI iDText;
    [SerializeField] TextMeshProUGUI missionDesc;
    [SerializeField] TextMeshProUGUI missionProgress;
    [SerializeField] Image panel;
    Color panelColor;
    Coroutine twinkle;
    

    [SerializeField] StatsSO statsSO;

    void Awake()
    {
        _instance = this;
        panelColor = panel.color;
    }

    void Start()
    {
        var missionData = DataManager.Instance.LoadUserData()?.missionData;

        if (missionData == null)
        {
            missionID = 0;
            kill = 0;
            earnedGold = 0;

        }
        else
        {
            missionID = missionData.missionID;
            kill = missionData.kill;
            earnedGold = BigInteger.Parse(missionData.earnedGold);

        }


        //DataManager.Instance.LoadMission(missinoList => missions = missinoList.missions);

        CurMission();
        curValue.Subscribe(value =>
        {
            if (curValue.Value == -1) return;

            missionProgress.text = $"({Utility.FormatNumberKoreanUnit(value)}/{Utility.FormatNumberKoreanUnit(goal)})";

            if (value >= goal && twinkle == null)
            {

                twinkle = StartCoroutine(Twinkle());


            }


        });
    }


    public void CurMission()
    {
        var length = DataManager.Instance.missions.Length;
        var mission = DataManager.Instance.missions[missionID % length];
        iDText.text = $"Mission {missionID + 1}";

        goal = BigInteger.Parse(mission.goal) + missionID / length;
        missionDesc.text = string.Format(mission.description, Utility.FormatNumberKoreanUnit(goal));

        missionType = mission.type;

        rewardType = mission.rewards.type;
        reward = mission.rewards.amount;

        MissionInfo();




    }


    void MissionInfo()
    {
        statType = StatType.None;
        curValue.Value = -1;
        switch (missionType)
        {
            case "Kill":
                if (isClear) kill = 0;
                isClear = false;
                curValue.Value = kill;

                return;
            case "EarnedGold":
                if (isClear) earnedGold = 0;
                isClear = false;
                curValue.Value = earnedGold;
                return;


        }

        if (Enum.TryParse<StatType>(missionType, out var parsedType))
        {
            statType = parsedType;
        }


        if (statType != StatType.None)
            StatMission(statType);

    }

    public void StatMission(StatType type)
    {
        if (statType != type)
            return;


        curValue.Value = statsSO.GetStat(type).level.Value;



    }

    public void Kill()
    {
        if (missionType != "Kill") return;
        kill++;
        curValue.Value = kill;



    }

    public void EarnedGold(BigInteger gold)
    {
        if (missionType != "EarnedGold") return;
        earnedGold += gold;
        curValue.Value = earnedGold;

    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (goal > curValue.Value)
            return;
        if (twinkle != null)
        {
            StopCoroutine(twinkle);
            twinkle = null;

        }


        panel.color = panelColor;


        missionID += 1;
        statType = StatType.None;
        CurMission();
        isClear = true;


        if (rewardType == "Gold")
        {
            statsSO.AddGold(reward);
        }
        else if (rewardType == "Exp")
        {
            statsSO.AddExp((int)reward);
        }

    }

    IEnumerator Twinkle()
    {

        while (true)
        {
            float t = Mathf.PingPong(Time.time, 1f);  // t = 0~1 사이
            float alpha = Mathf.Lerp(0.3f, 1f, t);  // t를 원하는 범위로 보간
            panel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }
    }

}