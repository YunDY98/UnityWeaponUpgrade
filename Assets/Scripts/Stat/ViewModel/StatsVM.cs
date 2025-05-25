
using System.Numerics;
using R3;
using System.Collections.Generic;
using Assets.Scripts;

public class StatsVM
{

    // public List<StatInfo> uList = new();
    // public List<UpgradeUI> showUIList = new();

    public event System.Action GoldWarningEvent;

    public List<StatInfo> statInfos = new();
    readonly StatsSO _model;

    public Stat GetStat(int type) => _model.GetStat(type); // 최대 체력, 공격력, 크리티컬 등 능력치
    public Stat GetStat(StatType type) => _model.GetStat(type);

    public ReactiveProperty<BigInteger> Gold => _model.Gold; // 플레이어 보유 골드
    public ReactiveProperty<BigInteger> CurHP => _model.CurHP; // 플레이어 현재 체력
    public ReactiveProperty<int> Exp => _model.Exp; // 플레이어 경험치
    public ReactiveProperty<int> Level => _model.Level; // 플레이어 레벨
    public ReactiveProperty<bool> IsDead = new(); // 플레이어가 죽었는지 체크
    public ReactiveProperty<int> statUpMult = new(1); // 스탯을 1,10,100씩 업그레이드


    public StatsVM(StatsSO model)
    {

        _model = model;
        CurHP.Subscribe(newHP =>
        {
            IsDead.Value = newHP <= 0;

        });


        statInfos.Clear();
        SetUpgradeUI();

    }




    public void SetStatUpMult(int mult)
    {
        // 1, 10 ,100씩 업그레이드 배율 조정

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
        // 골드가 있다면 
        if (UseGold(stat.cost.Value))
        {
            _model.IncreaseStat(stat, increase);
        }
        else
        {
            // 강화 중 골드 부족시 사운드 종료 
            if (AudioManager.Instance.longClickSound != null)
                AudioManager.Instance.longClickSound.Stop();
        }




    }


    public void TestGold()
    {
        Gold.Value += BigInteger.Parse("1000000000000000000000000000000000000");
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

            // 스탯 스프라이트
            ui.sprite = DataManager.Instance.statSprite[stat.key];

            // 업그레이드 후 스탯 레벨
            int nextLevel = 0;
            // 현재 스탯 레벨
            int curLevel = 0;

            // 스탯의 레벨과 업그레이드 배수가 바뀔때마다 반응
            Observable.CombineLatest(stat.level, statUpMult,
            (level, levelUpMult) => new { level, levelUpMult })
            .Subscribe(data =>
            {
                curLevel = data.level;
                nextLevel = data.level + data.levelUpMult;

                // 업그레드후 레벨이 맥스 레벨을 초과시 
                if (nextLevel > stat.maxLevel)
                {
                    nextLevel = stat.maxLevel;
                }

                // 현재 미션 중인 스탯의 레벨 표기 
                MissionManager.Instance.SetStatMission(stat.key);

                // 현재 스탯 ex) 공격력 100
                BigInteger curValue = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, curLevel);
                // 업그레이드 후 스탯 ex) 공격력 150
                BigInteger nextValue = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, nextLevel);
                // 업그레이드 비용 
                stat.cost.Value = Utility.GeometricSumInRange(stat.baseCost, stat.costRate, curLevel, nextLevel);
                // ui에 비용 표기 
                ui.cost.Value = Utility.FormatNumberKoreanUnit(stat.cost.Value);

                // 현재 레벨과 업그레드 후 레벨이 같은 경우 Max레벨
                if (curLevel == nextLevel)
                {
                    ui.cost.Value = "Max";
                }

                float scale = 1;

                // 스탯이 실수 인경우 ex) 99.9%
                if (stat.floatScale > 0)
                {
                    scale = stat.floatScale;

                    if (stat.format == "%")
                    {
                        // ex) 40% -> 50%
                        ui.description.Value = $"{(double)curValue / scale * 100}{stat.format} → {(double)nextValue / scale * 100}{stat.format}";
                    }
                    else
                    {
                        // ex) 4.5초 -> 5.5초 
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
            // 리스트에 추가 
            statInfos.Add(ui);

        }
    }


}


