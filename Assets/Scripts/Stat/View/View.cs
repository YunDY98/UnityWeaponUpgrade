using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using BigInteger = System.Numerics.BigInteger;
using System.Collections;




public class View : MonoBehaviour
{

    [SerializeField]
    GameObject uObject;

    #region multUpgrade
    [SerializeField]
    Button[] multBtn;

    #endregion



    [SerializeField]
    RectTransform uContent;

    StatsVM viewModel;

    WaitForSeconds wait =  new(1f);



    void Start()
    {
        viewModel = GameManager.Instance.statsVM;
       

       //StartCoroutine(FrameDelay());
       DrawUI();
       if(multBtn[0] == null) return;
       multBtn[0].onClick.AddListener(() => StatLevelUpMult(1,multBtn[0]));
       StatLevelUpMult(1,multBtn[0]);
       multBtn[1].onClick.AddListener(() => StatLevelUpMult(10,multBtn[1]));
       multBtn[2].onClick.AddListener(() => StatLevelUpMult(100,multBtn[2]));

       
    }



    void StatLevelUpMult(int x,Button btn)
    {
        viewModel.SetStatUpMult(x);
        foreach(var select in multBtn)
        {
            ColorBlock cb = select.colors;
            if(select == btn)
            {
               
                cb.normalColor = Color.black;
                cb.selectedColor = Color.black;
              
            }
            else
            {
                cb.normalColor = Color.white;
               
            }
            select.colors = cb;
        }

    }



    void CreateUpgradeUI(Stat stat)
    {
        var obj = Instantiate(uObject,uContent);
        var ui = obj.GetComponent<UpgradeUI>();

        
       
        string name = stat.textName;
        ui.statName.text = name;
        print($"StatIcon/{stat.key}");
        Utility.LoadSprite($"StatIcon/{stat.key}",ui.image);
        var btn = ui.btn;
        int nextLevel = 0;
        int curLevel = 0;

        btn.onClick.AddListener(() =>
        {
            viewModel.StatUpgrade(stat,viewModel.statUpMult.Value);

        } );

       
        
        Observable.CombineLatest(stat.level, viewModel.statUpMult,
        (level, levelUpMult) => new { level, levelUpMult })
        .Subscribe(data =>
        {   
            curLevel = data.level;
            nextLevel = data.level + data.levelUpMult;

            if(nextLevel > stat.maxLevel)
            {
                
                nextLevel = stat.maxLevel;
              
            }
            
            BigInteger curValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,curLevel);
          
            BigInteger nextValue = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,nextLevel);
            
            stat.cost.Value = Utility.GeometricSumInRange(stat.baseCost,stat.costRate,curLevel,nextLevel);
        
            float scale = 1;

            if(stat.floatScale > 0)
            {
                scale = stat.floatScale;

                ui.description.text = $"{(double)curValue / scale * 100} → {(double)nextValue / scale * 100}";

            }
            else
            {
                ui.description.text = $"{Utility.FormatNumberKoreanUnit(curValue)} → {Utility.FormatNumberKoreanUnit(nextValue)}";
                
            }

            ui.level.text = $"Lv.{curLevel}";
          
            
        }).AddTo(ui.sub);


        stat.cost.Subscribe(cost =>
        {
            if(curLevel >= stat.maxLevel)
            {
                ui.cost.text = "Max";
                btn.onClick.RemoveAllListeners();
                ui.longClick.enabled = false;
                btn.interactable = false; 

                return;
            }
           
            ui.cost.text = Utility.FormatNumberKoreanUnit(cost);
            

           
        }).AddTo(ui.sub);

        ui.maxLevelText.text = $"(max: {stat.maxLevel})";
         
    }

    public void TestGold()
    {
        viewModel.TestGold();
    }

   
    public void DrawUI()
    {
       
       if(uObject == null) return;

        RectTransform uObj = uObject.GetComponent<RectTransform>();
       
        foreach(var stat in viewModel.GetStats())
        {
            CreateUpgradeUI(stat);
            
        }
        uContent.sizeDelta = new Vector2(uContent.sizeDelta.x,viewModel.GetStats().Length * uObj.sizeDelta.y * 1.6f);
    }



}
