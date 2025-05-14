
using System.Numerics;
using R3;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts;
using System;

public class StatsVM
{

    // public List<StatInfo> uList = new();
    // public List<UpgradeUI> showUIList = new();

    public event Action GoldWarningEvent;

    public List<StatInfo> datas = new();
    readonly StatsSO _model;

    public Stat GetStat(int type) => _model.GetStat(type);
    public Stat GetStat(StatType type) => _model.GetStat(type);




    public ReactiveProperty<BigInteger> Gold => _model.Gold;


    public ReactiveProperty<BigInteger> CurHP => _model.CurHP;

    public ReactiveProperty<int> Exp => _model.Exp;

    public ReactiveProperty<int> Level => _model.Level;

    public ReactiveProperty<bool> IsDead = new();

    public ReactiveProperty<int> statUpMult = new(1);


    public StatsVM(StatsSO model)
    {

        _model = model;
        CurHP.Subscribe(newHP =>
        {
            IsDead.Value = newHP <= 0;

        });

        RetrieveData();

    }



    public void SetStatUpMult(int mult)
    {

        statUpMult.Value = mult;
    }


    public Stat[] GetStats()
    {


        return _model.GetStats();


    }



    public bool UseGold(BigInteger useGold)
    {
        if (_model.Gold.Value >= useGold)
        {
            _model.UseGold(useGold);
            return true;

        }
        GoldWarningEvent?.Invoke();
        return false;

    }

    public void AddGold(BigInteger gold)
    {
        _model.AddGold(gold);
    }


    public void StatUpgrade(Stat stat, int increase = 1)
    {

        if (UseGold(stat.cost.Value))
        {
            _model.IncreaseStat(stat, increase);
        }
        else
        {
            if(AudioManager.Instance.longClickSound != null)
                AudioManager.Instance.longClickSound.Stop();
        }
        



    }


    public void TestGold()
    {
        Gold.Value += BigInteger.Parse("1000000000000000000000000000000000000");
    }

    private void RetrieveData()
    {
        datas.Clear();

        SetUpgradeUI();


    }


    public void SetUpgradeUI()
    {
        foreach (var stat in GetStats())
        {

            StatInfo ui = new()
            {
                type = stat.key,

                statName = stat.textName,

            };

            Utility.LoadSprite($"StatIcon/{stat.key}", (sprite) => ui.sprite = sprite);

            int nextLevel = 0;
            int curLevel = 0;


            Observable.CombineLatest(stat.level, statUpMult,
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

                if (curLevel == nextLevel)
                {
                    ui.cost.Value = "Max";
                   
                }

                float scale = 1;

                if (stat.floatScale > 0)
                {
                    scale = stat.floatScale;

                    

                    if(stat.format == "%")
                    {
                        ui.description.Value = $"{(double)curValue / scale * 100}{stat.format} → {(double)nextValue / scale * 100}{stat.format}";
                    }
                    else
                    {
                        ui.description.Value = $"{(double)curValue / scale}{stat.format} → {(double)nextValue / scale}{stat.format}";
                    }

                    

                }
                else
                {
                    ui.description.Value = $"{Utility.FormatNumberKoreanUnit(curValue)}{stat.format} → {Utility.FormatNumberKoreanUnit(nextValue)}{stat.format}";

                }

                ui.level.Value = curLevel.ToString();


            });

            ui.maxLevelText = stat.maxLevel.ToString();

            datas.Add(ui);

        }
    }


}
