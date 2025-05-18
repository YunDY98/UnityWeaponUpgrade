using System.Numerics;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;


public class MissionManager : MonoBehaviour, IPointerDownHandler
{
    private static MissionManager _instance;

    public static MissionManager Instance
    {
        get => _instance;
    }

    public int killCnt = 0;
    public int earnedGold = 0;
    int ID => statsSO.missionID;

    ReactiveProperty<int> curValue = new();
    int goal;
    //int curValue;
    string missionType;
    StatType statType;
    string rewardType;
    BigInteger reward = new();
    
    bool isClear = false;



    [SerializeField] TextMeshProUGUI iDText;
    [SerializeField] TextMeshProUGUI missionDesc;
    [SerializeField] TextMeshProUGUI missionProgress;
    MissionData[] missions;

    [SerializeField] StatsSO statsSO;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        missions = DataManager.Instance.Mission().missions;
        CurMission();
        curValue.Subscribe(value => missionProgress.text = $"({value}/{goal})");
    }


    public void CurMission()
    {
        
        var mission = missions[ID % missions.Length];
        iDText.text = $"Mission {ID}";
       
        goal = mission.goal + ID / missions.Length;
        missionDesc.text = string.Format(mission.description, goal); 
        missionType = mission.type;

        rewardType = mission.rewards.type;
        reward = mission.rewards.amount;

        MissionInfo();




    }


    void MissionInfo()
    {
        statType = StatType.None;
        switch (missionType)
        {
            case "Kill":
                if(isClear) statsSO.kill = 0;
                isClear = false;
                killCnt = statsSO.kill;
                curValue.Value = killCnt;
                
                break;
            case "EarnedGold":
                if(isClear) statsSO.earnedGold = 0;
                isClear = false;
                earnedGold = statsSO.earnedGold;
                curValue.Value = earnedGold;
                break;

           
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
        killCnt += 1;
        curValue.Value = killCnt;

        

    }

    public void EarnedGold(int gold = 0)
    {
        if (missionType != "EarnedGold") return;
        earnedGold += gold;
        curValue.Value = earnedGold;

    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (goal > curValue.Value)
            return;

        statsSO.missionID += 1;
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

}