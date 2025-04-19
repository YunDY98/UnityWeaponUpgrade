using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using System.Numerics;
using System.Collections.Generic;
using System;



public class View : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    TextMeshProUGUI goldText;

    [SerializeField]
    GameObject uObject;

    [SerializeField]
    RectTransform uContent;

    PlayerVM viewModel;
    [SerializeField]
    StatsSO model;

   
    void Awake()
    {
        viewModel = new(this,model);
       

        //atkUpButton.onClick.AddListener(() => viewModel.UpgradeStat()); // 공격력 업그레이드 
       // atkUpButton.AddComponent<LongClick>();

        
    }

    void Start()
    {
        model.CurHP.Subscribe(newHP => 
        {
            hpSlider.value = (float)((double)newHP / (double)viewModel.GetStat((int)StatType.MaxHP).value.Value);

           
            

        });

        viewModel.Gold.Subscribe(newGold => goldText.text = Utility.FormatNumberKoreanUnit(newGold)); // 골드 표기
        foreach(var stat in viewModel.GetStats())
        {
            CreateUpgradeUI(stat);
        }
    }



    void CreateUpgradeUI(Stat stat)
    {
        var obj = Instantiate(uObject,uContent);
        var ui = obj.GetComponent<UpgradeUI>();
       
        string name = stat.textName;
        ui.statName.text = name;
        //ui.image.sprite = null;
        var btn = ui.btn;
     
        stat.level.Subscribe(level => 
        {
            BigInteger curValue = stat.value.Value;
            BigInteger nextValue = curValue + (int)(stat.upgradeRate * stat.level.Value);
            
           
            ui.description.text = $"{Utility.FormatNumberKoreanUnit(curValue)} → {Utility.FormatNumberKoreanUnit(nextValue)}";
            ui.level.text = $"Lv.{Utility.FormatNumberKoreanUnit(level)}";
            
            
        });

        stat.cost.Subscribe(cost =>{ui.cost.text = Utility.FormatNumberKoreanUnit(cost);});


        //value.value.Subscribe(newVlaue => ui.cost = Utility.FormatNumberKoreanUnit(newValue));
        
        btn.onClick.AddListener(() => viewModel.BigIntStatUpgrade(stat));
         
    }

   


    
   
}
