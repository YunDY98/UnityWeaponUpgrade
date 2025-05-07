
using System.Collections.Generic;
using UnityEngine;
using R3;
using Assets.Scripts;
using System.Numerics;
using System.Collections;


public class TestPanel : MonoBehaviour
{
    public RecyclingListView theList;
    private List<StatInfo> datas = new();
    
    public StatsSO statsSO;

    StatsVM viewModel;

    private void Start()
    {
        

        viewModel = GameManager.Instance.statsVM;

        RetrieveData();

        theList.ItemCallback = PopulateItem;

        // This will resize the list and cause callbacks to PopulateItem for
        // items that are needed for the view

        
        StartCoroutine(WaitForLoading());




    }

    private void RetrieveData()
    {
        datas.Clear();
        
        SetUpgradeUI();


    }
    IEnumerator WaitForLoading()
    {
        bool isSprite = false;
        while (!isSprite)
        {
            foreach (var sprite in datas)
            {
                if (sprite.sprite == null)
                {
                    isSprite = false;
                    break;


                }
                isSprite = true;


            }
            yield return null;

        }
        theList.RowCount = datas.Count;

    }

    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {

        var child = item as UpgradeUI;
        var data = datas[rowIndex];
        child.statName.text = data.statName;
        child.image.sprite = data.sprite;

        child.sub.Clear();

        data.cost.Subscribe(x => child.cost.text = x).AddTo(child.sub);
        data.level.Subscribe(x => child.level.text = x).AddTo(child.sub);

        child.maxLevelText.text = data.maxLevelText;

        data.description.Subscribe(x => child.description.text = x).AddTo(child.sub);


        child.btn.onClick.RemoveAllListeners();

        if (data.level.Value == data.maxLevelText)
        {
           
            return;
        }
           

        child.btn.onClick.AddListener(() => viewModel.StatUpgrade(viewModel.GetStat(data.type), viewModel.statUpMult.Value));

    }

    public void SetUpgradeUI()
    {



        foreach (var stat in statsSO.GetStats())
        {

            StatInfo ui = new()
            {
                type = stat.key,

                statName = stat.textName,


            };

            Utility.LoadSprite($"StatIcon/{stat.key}", (sprite) => ui.sprite = sprite);




            int nextLevel = 0;
            int curLevel = 0;


            Observable.CombineLatest(stat.level, viewModel.statUpMult,
            (level, levelUpMult) => new { level, levelUpMult })
            .Subscribe(data =>
            {


                curLevel = data.level;
                nextLevel = data.level + data.levelUpMult;

                if (nextLevel > stat.maxLevel)
                {

                    nextLevel = stat.maxLevel;

                }

                BigInteger curValue = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, curLevel);

                BigInteger nextValue = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, nextLevel);

                stat.cost.Value = Utility.GeometricSumInRange(stat.baseCost, stat.costRate, curLevel, nextLevel);

                ui.cost.Value = Utility.FormatNumberKoreanUnit(stat.cost.Value);

                float scale = 1;

                if (stat.floatScale > 0)
                {
                    scale = stat.floatScale;

                    ui.description.Value = $"{(double)curValue / scale * 100} → {(double)nextValue / scale * 100}";

                }
                else
                {
                    ui.description.Value = $"{Utility.FormatNumberKoreanUnit(curValue)} → {Utility.FormatNumberKoreanUnit(nextValue)}";

                }

                ui.level.Value = $"Lv.{curLevel}";


            });




            ui.maxLevelText = $"(max: {stat.maxLevel})";


            datas.Add(ui);

        }
    }

}
