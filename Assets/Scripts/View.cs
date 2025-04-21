using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using System.Numerics;




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
    



    void Start()
    {
        viewModel = GameManager.Instance.playerVM;
        viewModel.Gold.Subscribe(newGold => goldText.text = Utility.FormatNumberKoreanUnit(newGold)); // 골드 표기
        viewModel.CurHP.Subscribe(newHP => 
        {
            hpSlider.value = (float)((double)newHP / (double)viewModel.GetStat((int)StatType.MaxHP).value.Value);

           
            

        });

        RectTransform uObj = uObject.GetComponent<RectTransform>();
       
        foreach(var stat in viewModel.GetStats())
        {
            CreateUpgradeUI(stat);
        }
        uContent.sizeDelta = new UnityEngine.Vector2(uContent.sizeDelta.x,viewModel.GetStats().Length * uObj.sizeDelta.y * 1.6f);
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
   


    
   
}
