using System.Numerics;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;


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
    [SerializeField] Image panel;
    Color panelColor;
    Coroutine twinkle;
    MissionData[] missions;

    [SerializeField] StatsSO statsSO;

    void Awake()
    {
        _instance = this;
        panelColor = panel.color;
    }

    void Start()
    {
        missions = DataManager.Instance.Mission().missions;
        CurMission();
        curValue.Subscribe(value => {
            missionProgress.text = $"({value}/{goal})";

            if(value >= goal && twinkle == null)
            {
                
                twinkle = StartCoroutine(Twinkle());
                panel.color = panelColor;


            }
                
            
        });
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

        StopCoroutine(twinkle);
        twinkle = null;

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

   IEnumerator Twinkle()
    {
    
        while (true)
        {
            float t = Mathf.PingPong(Time.time, 1f);  // t = 0~1 사이
            float alpha = Mathf.Lerp(0.3f, 0.8f, t);  // t를 원하는 범위로 보간
            panel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            yield return null;
        }
    }

}