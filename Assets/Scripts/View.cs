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
    Button atkUpButton;
    [SerializeField]
    TextMeshProUGUI aktUpText;

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
        model.CurHP.Subscribe(newHP => 
        {
            hpSlider.value = (float)((double)newHP / (double)model.MaxHP.value.Value);

        });

        //atkUpButton.onClick.AddListener(() => viewModel.UpgradeStat()); // 공격력 업그레이드 
       // atkUpButton.AddComponent<LongClick>();

        viewModel.Gold.Subscribe(newGold => goldText.text = Utility.FormatNumberKoreanUnit(newGold)); // 골드 표기
        foreach(var dic in viewModel.BigIntStatDic)
        {
            CreateUpgradeUI(dic);
        }
    }


    void CreateUpgradeUI(KeyValuePair<string,Stat<BigInteger>> dic)
    {
        var obj = Instantiate(uObject,uContent);
        var ui = obj.GetComponent<UpgradeUI>();
        var value = dic.Value;
        string name = value.textName;
        ui.statName.text = name;
        //ui.image.sprite = null;
        var btn = ui.button;
     
        value.level.Subscribe(level => 
        {
            BigInteger curValue = value.value.Value;
            BigInteger nextValue = curValue + (int)(value.upgradeRate * value.level.Value);
            
           
            ui.description.text = $"{Utility.FormatNumberKoreanUnit(curValue)} → {Utility.FormatNumberKoreanUnit(nextValue)}";
            ui.level.text = $"Lv.{Utility.FormatNumberKoreanUnit(level)}";
            
            
        });

        value.cost.Subscribe(cost =>{ui.cost.text = Utility.FormatNumberKoreanUnit(cost);});


        //value.value.Subscribe(newVlaue => ui.cost = Utility.FormatNumberKoreanUnit(newValue));
        
        btn.onClick.AddListener(() => viewModel.BigIntStatUpgrade(dic));
         
    }

   


    
   
}
