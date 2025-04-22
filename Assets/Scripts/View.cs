using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using System.Numerics;
using System.Collections;
using UnityEditor;
using UnityEditor.Rendering;




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
    RectTransform uContent;

    PlayerVM viewModel;

    WaitForSeconds wait =  new(1f);

    void Start()
    {
        viewModel = GameManager.Instance.playerVM;
       

       StartCoroutine(FrameDelay());
       
    }

    void Update()
    {
        print(viewModel.Exp.Value);
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
            
            BigInteger curValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,level);
            
            BigInteger nextValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,level+1);

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

            ui.level.text = $"Lv.{level}";
           
           
            
            
        });

        stat.cost.Subscribe(cost =>{ui.cost.text = Utility.FormatNumberKoreanUnit(cost);});


        //value.value.Subscribe(newVlaue => ui.cost = Utility.FormatNumberKoreanUnit(newValue));
        
        btn.onClick.AddListener(() => viewModel.BigIntStatUpgrade(stat));
         
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
