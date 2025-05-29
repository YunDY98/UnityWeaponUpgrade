using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeUI : RecyclingListViewItem
{
    public CompositeDisposable sub = new();// 재사용을 위한 구독 정보 
    public TextMeshProUGUI statName;
    public TextMeshProUGUI cost;
    public Button btn;
    public TextMeshProUGUI description;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI maxLevelText;
    public Image image;
    

}

public class StatInfo
{

    public StatType type;
    public string statName;
    public ReactiveProperty<string> cost = new();
    public ReactiveProperty<string> description = new();
    public ReactiveProperty<string> level = new();
    public string maxLevelText;
    public LongClick longClick;
    public Sprite sprite;

}
