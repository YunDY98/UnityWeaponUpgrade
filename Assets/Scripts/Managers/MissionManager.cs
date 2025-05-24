using System.Numerics;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts;




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
    BigInteger goal = new(); // 미션 목표
    //int curValue;
    string missionType;
    StatType statType;
    string rewardType;
    BigInteger reward = new();

    bool isClear = false; // 미션 클리어 여부 





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

        SetMission();
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


    public void SetMission()
    {
        // 미션 배열 크기 
        var length = DataManager.Instance.missions.Length;
        // 0 ~ length - 1번째 배열 
        var mission = DataManager.Instance.missions[missionID % length];

        iDText.text = $"Mission {missionID + 1}";

        // 미션 단계만큼 목표 상승 
        goal = BigInteger.Parse(mission.goal) + missionID / length;
        missionDesc.text = string.Format(mission.description, Utility.FormatNumberKoreanUnit(goal));

        // 어떤 미션인지 
        missionType = mission.type;
        // 어떤 보상인지 
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
        // string missinoType -> StatType
        if (Enum.TryParse<StatType>(missionType, out var parsedType))
        {
            statType = parsedType;
        }

        // 스탯과 관련된 미션 
        if (statType != StatType.None)
            SetStatMission(statType);

    }

    public void SetStatMission(StatType type)
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

        // 깜빡거리기 전 컬러
        panel.color = panelColor;

        // 클리어시 미션 아이디 + 1
        missionID += 1;

        // 미션 재설정
        SetMission();
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





