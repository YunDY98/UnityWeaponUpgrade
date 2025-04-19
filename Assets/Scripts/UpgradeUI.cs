using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Numerics;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI cost;
    public Button btn;
    public TextMeshProUGUI description;

    public TextMeshProUGUI level;

    public Image image;

    // Stat<T> _targetStat;
    // Action<Stat<T>> _upgradeAction;

    //  public void Init(Stat<T> stat, Action<Stat<T>> upgradeAction, Sprite icon = null)
    // {
    //     _targetStat = stat;
    //     _upgradeAction = upgradeAction;
        
    //     statName.text = stat.textName;
    //     if (icon != null) image.sprite = icon;
        
    //     UpdateUI();
        
    //     // ReactiveProperty 구독
    //     stat.value.Subscribe(_ => UpdateUI());
    //     stat.level.Subscribe(_ => UpdateUI());
    //     stat.cost.Subscribe(_ => UpdateUI());
        
    //     btn.onClick.AddListener(() => _upgradeAction?.Invoke(_targetStat));
    // }


    // private void UpdateUI()
    // {
    //     level.text = $"Lv.{Utility.FormatNumberKoreanUnit(_targetStat.level.Value)}";
    //     cost.text = Utility.FormatNumberKoreanUnit(_targetStat.cost.Value);
        
       
    //     BigInteger current = (BigInteger)(object)_targetStat.value.Value;
    //     BigInteger next = current + (BigInteger)(_targetStat.level.Value * _targetStat.upgradeRate);
    //     description.text = $"{Utility.FormatNumberKoreanUnit(current)} → {Utility.FormatNumberKoreanUnit(next)}";
        
      
    // }










   
}

