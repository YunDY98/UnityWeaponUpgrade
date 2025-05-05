using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using System.Collections;




public class HUD : MonoBehaviour
{
    [SerializeField]
    GameObject levelUP;
    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    Slider expSlider;

    [SerializeField]
    TextMeshProUGUI goldText;
    [SerializeField]
    TextMeshProUGUI levelText;

    StatsVM viewModel;

    WaitForSeconds wait =  new(1f);



    void Start()
    {
        viewModel = GameManager.Instance.statsVM;
       

       //StartCoroutine(FrameDelay());
       DrawUI();
      
       
    }
    IEnumerator LevelUp()
    {
        levelUP.SetActive(true);
        yield return wait;
        levelUP.SetActive(false);
    }
    public void DrawUI()
    {
        viewModel.Gold.Subscribe(Gold => goldText.text = Utility.FormatNumberKoreanUnit(Gold)); // 골드 표기


        Observable.CombineLatest(viewModel.CurHP, viewModel.GetStat(StatType.MaxHP).value,
        (curHP, maxHP) => new { curHP, maxHP })
        .Subscribe(data => 
        {
            hpSlider.value = (float)((double)data.curHP / (double)data.maxHP);

           
            
        });
        
        viewModel.Exp.Subscribe(exp => 
        {
            expSlider.value = (float)exp / (float)viewModel.Level.Value;


        });

        viewModel.Level.Subscribe(level =>
        {
            levelText.text = $"Lv.{level}";
           

        });
        viewModel.Level.Skip(1).Subscribe(level =>
        {
            StartCoroutine(LevelUp());
        
        });
       
    }



}
