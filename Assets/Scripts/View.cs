using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using System.Numerics;
using System.Collections;




public class View : MonoBehaviour
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

    [SerializeField]
    GameObject uObject;
    [SerializeField]
    Button x1;
    [SerializeField]
    Button x10;
    [SerializeField]
    Button x100;

    [SerializeField]
    RectTransform uContent;

    PlayerVM viewModel;

    WaitForSeconds wait =  new(1f);

   

    void Start()
    {
        viewModel = GameManager.Instance.playerVM;
       

       StartCoroutine(FrameDelay());
       x1.onClick.AddListener(() =>StatLevelUpMult(1));
       x10.onClick.AddListener(() =>StatLevelUpMult(10));
       x100.onClick.AddListener(() =>StatLevelUpMult(100));

       
    }



    void StatLevelUpMult(int x)
    {
        viewModel.statLevelUpMult.Value = x;

    }



    void CreateUpgradeUI(Stat stat)
    {
        var obj = Instantiate(uObject,uContent);
        var ui = obj.GetComponent<UpgradeUI>();
       
        string name = stat.textName;
        ui.statName.text = name;
        //ui.image.sprite = null;
        var btn = ui.btn;

        
     
        Observable.CombineLatest(stat.level, viewModel.statLevelUpMult,
        (level, levelUpMult) => new { level, levelUpMult })
        .Subscribe(data =>
        {
            
            BigInteger curValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,data.level);
            
            BigInteger nextValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,data.level + data.levelUpMult);

            stat.cost.Value = Utility.GeometricSumInRange(stat.baseCost,stat.costRate,data.level,data.level+data.levelUpMult);
        
            float scale = 1;

            if(stat.floatScale > 0)
            {
                scale = stat.floatScale;

                ui.description.text = $"{(double)curValue / scale} → {(double)nextValue / scale}";

                
            }
            else
            {
                ui.description.text = $"{Utility.FormatNumberKoreanUnit(curValue)} → {Utility.FormatNumberKoreanUnit(nextValue)}";
                

            }

            ui.level.text = $"Lv.{data.level + 1}";
           
           
            
            
        });


        stat.cost.Subscribe(cost =>{ui.cost.text = Utility.FormatNumberKoreanUnit(cost);});

        
        btn.onClick.AddListener(() => viewModel.StatUpgrade(stat,viewModel.statLevelUpMult.Value));
         
    }

    public void TestGold()
    {
        viewModel.TestGold();
    }

    IEnumerator FrameDelay()
    {
        yield return null;
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
        viewModel.CurHP.Subscribe(HP => 
        {
            hpSlider.value = (float)((double)HP / (double)viewModel.GetStat(StatType.MaxHP).value.Value);

           
            

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
       
       

        RectTransform uObj = uObject.GetComponent<RectTransform>();
       
        foreach(var stat in viewModel.GetStats())
        {
            CreateUpgradeUI(stat);
            
        }
        uContent.sizeDelta = new UnityEngine.Vector2(uContent.sizeDelta.x,viewModel.GetStats().Length * uObj.sizeDelta.y * 1.6f);
    }
        
    




}
