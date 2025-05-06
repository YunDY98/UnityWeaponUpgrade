using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class UpgradeUI : MonoBehaviour
{
    public CompositeDisposable sub = new();
    public TextMeshProUGUI statName;
    public TextMeshProUGUI cost;
    public Button btn;
    public TextMeshProUGUI description;
    public TextMeshProUGUI level;
    public TextMeshProUGUI maxLevelText;
    public LongClick longClick;
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
