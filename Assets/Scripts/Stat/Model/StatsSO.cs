using System;
using System.Numerics;
using Assets.Scripts;
using R3;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "StatsSO", menuName = "ScriptableObjects/Player", order = 1)]

public class StatsSO : ScriptableObject
{

    Stat[] stats;

    public ReactiveProperty<BigInteger> CurHP;
    public ReactiveProperty<int> Level;
    public ReactiveProperty<int> Exp;
    public ReactiveProperty<BigInteger> Gold;

   

    public void Init(UserData uData)
    {
        stats = new Stat[Enum.GetValues(typeof(StatType)).Length-1];
       
       
        {
            stats[(int)StatType.AttackDamage] = new()
            {
                value = new(1000),
                baseValue = 1000,
                key = StatType.AttackDamage,
                textName = "공격력",
                baseCost = 1000,
                cost = new(1000),
                costRate = 1.001f,
                upgradeRate = 1.005f,
                level = new(1),
                maxLevel = 10000,
                floatScale = 0
            };

            stats[(int)StatType.MaxHP] = new()
            {
                value = new(1000),
                baseValue = 1000,
                key = StatType.MaxHP,
                textName = "최대 체력",
                baseCost = 10000,
                cost = new(BigInteger.Parse("1000")),
                costRate = 1.01f,

                upgradeRate = 1,
                level = new(1),
                maxLevel = 1000,
                floatScale = 0
            };

            stats[(int)StatType.AttackRange] = new()
            {
                value = new(1000),
                baseValue = 1000,
                key = StatType.AttackRange,
                textName = "공격 범위",
                format = "배",
                baseCost = 1000,
                cost = new(BigInteger.Parse("1000000000")),
                costRate = 1.5f,
                upgradeRate = 1.01f,
                level = new(1),
                maxLevel = 100,
                floatScale = 1000
            };

            stats[(int)StatType.AttackCnt] = new()
            {
                value = new(1),
                baseValue = 1,
                key = StatType.AttackCnt,
                textName = "공격 마리 수",
                format = "마리",
                baseCost = 1000000000,
                cost = new(BigInteger.Parse("1000000000")),
                costRate = 10000,
                upgradeRate = 1,
                level = new(1),
                maxLevel = 5,
                floatScale = 0
            };


            stats[(int)StatType.AttackSpeed] = new()
            {
                value = new(3000),
                baseValue = 3000,
                key = StatType.AttackSpeed,
                textName = "공격 속도",
                format = "초",
                baseCost = 2000000000,
                cost = new(BigInteger.Parse("10000000")),
                costRate = 1.03f,
                upgradeRate = 0.999f,
                level = new(1),
                maxLevel = 1000,
                floatScale = 1000
            };

            stats[(int)StatType.AddGoldAmount] = new()
            {
                value = new(1000),
                baseValue = 1000,
                key = StatType.AddGoldAmount,
                textName = "골드 획득량",
                format = "%",
                baseCost = 1000,
                cost = new(BigInteger.Parse("1000")),
                costRate = 1.01f,
                upgradeRate = 1.001f,
                level = new(1),
                maxLevel = 100000,
                floatScale = 1000
            };

            stats[(int)StatType.StunTime] = new()
            {
                value = new(1),
                baseValue = 1,
                key = StatType.StunTime,
                textName = "스턴 지속시간",
                format = "초",
                baseCost = 1000,
                cost = new(BigInteger.Parse("1000000")),
                costRate = 1.03f,
                upgradeRate = 1,
                level = new(1),
                maxLevel = 10000,
                floatScale = 1000
            };

            stats[(int)StatType.StunRate] = new()
            {
                value = new(1),
                baseValue = 1,
                key = StatType.StunRate,
                textName = "넉백 확률",
                format = "%",
                baseCost = 1000,
                cost = new(BigInteger.Parse("100000")),
                costRate = 1.03f,
                upgradeRate = 1,
                level = new(1),
                maxLevel = 1000,
                floatScale = 1000
            };

            stats[(int)StatType.CriticalRate] = new()
            {
                value = new(10),
                baseValue = 10,
                key = StatType.CriticalRate,
                textName = "크리티컬 확률",
                format = "%",
                baseCost = 1000000,
                cost = new(BigInteger.Parse("1000000")),
                costRate = 1.03f,
                upgradeRate = 1f,
                level = new(1),
                maxLevel = 100,
                floatScale = 1000
            };

            stats[(int)StatType.CriticalDamage] = new()
            {
                value = new(1001),
                baseValue = 1001,
                key = StatType.CriticalDamage,
                textName = "크리티컬 데미지",
                format = "%",
                baseCost = 1000,
                cost = new(BigInteger.Parse("100000")),
                costRate = 2f,
                upgradeRate = 1.05f,
                level = new(1),
                maxLevel = 1000,
                floatScale = 1000
            };

        }
        if (uData != null)
        {
            Level = new(uData.userLevel);
            Exp = new(uData.userExp);
            Gold = new(BigInteger.Parse(uData.gold));

            for(int i=0; i<stats.Length;++i)
            {
                var stat = stats[i];
             
                stat.level.Value = uData.statData[i].level;
                stat.cost.Value = Utility.GeoProgression(stat.baseCost, stat.costRate, stat.level.Value);
                stat.value.Value = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, stat.level.Value);

            }
            CurHP = new(GetStat(StatType.MaxHP).value.Value);
          
        }
        else
        {
            
            CurHP = new(stats[(int)StatType.MaxHP].baseValue);

            Level = new(1);
            Exp = new(0);
            Gold = new(BigInteger.Parse("1000000"));

        }
      
       
    }

    public Stat GetStat(int type)
    {
        return stats[type];
    }

    public Stat GetStat(StatType type)
    {
        return stats[(int)type];
    }

    public Stat[] GetStats()
    {
        return stats;
    }

    public void IncreaseStat(Stat stat, int increase = 1)
    {


        stat.LevelUp(increase);

        stat.value.Value = Utility.GeoProgression(stat.baseValue, stat.upgradeRate, stat.level.Value);


    }

    FinalDamage fDamage = new();

    public FinalDamage Damage()
    {

        BigInteger damage = stats[(int)StatType.AttackDamage].value.Value;
        Stat cDamage = stats[(int)StatType.CriticalDamage];

        double rand = Random.value;

        Debug.Log(rand + "  :" + stats[(int)StatType.CriticalRate].GetFValue());
        if (rand < stats[(int)StatType.CriticalRate].GetFValue())
        {

            BigInteger criticalDamage = damage * cDamage.value.Value / cDamage.floatScale;
            fDamage.damage = criticalDamage;
            fDamage.isCritical = true;

            return fDamage;
        }
        fDamage.damage = damage;
        fDamage.isCritical = false;

        return fDamage;
    }


    public void LevelUp()
    {

        while (Exp.Value >= Level.Value)
        {
            Exp.Value -= Level.Value;
            Level.Value++;
        }

    }

    public void AddExp(int exp)
    {

        Exp.Value += exp;
        if (Exp.Value >= Level.Value)
            LevelUp();

        DataManager.Instance.SaveData();
    }

    public void AddGold(BigInteger gold)
    {
        MissionManager.Instance.EarnedGold((int)gold);
        Gold.Value += gold;
    }

    public void UseGold(BigInteger useGold)
    {

        Debug.Log("골드 사용 " + useGold);
        Gold.Value -= useGold;
        DataManager.Instance.SaveData();
    }




}

public struct FinalDamage
{
    public BigInteger damage;
    public bool isCritical;
    public FinalDamage(BigInteger damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }
}

public class Stat
{
    public StatType key;
    public string textName;
    public string format = "";
    public ReactiveProperty<BigInteger> value;


    public ReactiveProperty<int> level;

    public void LevelUp(int increase)
    {

       
        if (level.Value + increase > maxLevel)
        {
            level.Value = maxLevel;
        }
        else
        {
            level.Value += increase;
        }


    }

    public int maxLevel;

    public int floatScale;

    public int baseValue;



    public int baseCost;
    public ReactiveProperty<BigInteger> cost;
    public float costRate;
    public float upgradeRate;

    public Stat() { }

    // public Stat(BigInteger value,StatType key,string textName,BigInteger cost ,float costRate = 1,float upgradeRate = 1,int level = 1,int floatScale = 0)
    // {
    //     this.value = new (value);
    //     this.key = key;
    //     this.textName = textName;
    //     this.level = new (level);
    //     this.cost = new(cost);
    //     this.floatScale = floatScale;
    //     this.costRate = costRate;
    //     this.upgradeRate = upgradeRate;
    // }

    public double GetFValue()
    {
        if (floatScale == 0)
        {
            throw new InvalidOperationException("floatScale이 0이므로 GetFValue()를 호출할 수 없습니다.");
        }

        double fValue = (int)value.Value / (double)floatScale;


        return fValue;


    }

}

public enum StatType
{
    AttackDamage,
    MaxHP,
    AttackRange,
    AttackCnt,
    AttackSpeed,
    AddGoldAmount,
    StunTime,
    StunRate,
    CriticalRate,
    CriticalDamage,
    None

}















