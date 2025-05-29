using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using DG.Tweening;




public class HUD : MonoBehaviour
{

    
    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    Slider expSlider;

    [SerializeField]
    TextMeshProUGUI goldText;
    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField] float tweenDuration = 0.3f;

    StatsVM viewModel;

  

    void Start()
    {
        viewModel = GameManager.Instance.statsVM;

        DrawUI();


    }
    
    public void DrawUI()
    {
        viewModel.Gold.Subscribe(Gold => goldText.text = Utility.FormatNumberKoreanUnit(Gold)); // 골드 표기

        Observable.CombineLatest(
            viewModel.CurHP,
            viewModel.GetStat(StatType.MaxHP).value,
            (curHP, maxHP) => new { curHP, maxHP }
        )
        .Subscribe(data =>
        {
            float ratio = (float)((double)data.curHP / (double)data.maxHP);
            hpSlider.DOValue(ratio, tweenDuration).SetEase(Ease.OutCubic);
        });

        viewModel.Exp.Subscribe(exp =>
        {
            float ratio = (float)exp / (float)viewModel.Level.Value;
            expSlider.DOValue(ratio, tweenDuration).SetEase(Ease.OutCubic);
        });

        viewModel.Level.Subscribe(level =>
        {
            levelText.text = $"Lv.{level}";


        });
        

    }



}
