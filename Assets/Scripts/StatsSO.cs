using System;
using System.Collections.Generic;
using System.Numerics;
using R3;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
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
        stats = new Stat[Enum.GetValues(typeof(StatType)).Length];

        if(uData == null)
        {
            
            stats[(int)StatType.MaxHP] = new(1000, StatType.MaxHP, "최대 체력", 10, 1, 1, 1);
            stats[(int)StatType.AttackRange] = new(3000, StatType.AttackRange, "공격 범위", 10, 1, 1, 1, 1000);
            stats[(int)StatType.AttackCnt] = new(10, StatType.AttackCnt, "공격 가능 수", 10000000000, 1, 1, 1);
            stats[(int)StatType.AttackDamage] = new(1000, StatType.AttackDamage, "공격력", 100, 1, 1, 1);
            stats[(int)StatType.AttackSpeed] = new(1000, StatType.AttackSpeed, "공격 속도", 10, 1, 1, 1, 1000);
            stats[(int)StatType.AddGoldAmount] = new(1000, StatType.AddGoldAmount, "골드 획득량", 100000, 1, 1, 1, 1000);
            stats[(int)StatType.StunTime] = new(1000, StatType.StunTime, "스턴 지속시간", 10, 1, 1, 1, 1000);
            stats[(int)StatType.StunRate] = new(1000, StatType.StunRate, "스턴 확률", 10, 1, 1, 1, 1000);
            stats[(int)StatType.CriticalRate] = new(1000, StatType.CriticalRate, "크리티컬 확률", 10, 1, 1, 1, 1000);
            stats[(int)StatType.CriticalDamage] = new(1000, StatType.CriticalDamage, "크리티컬 데미지", 10, 1, 1, 1, 1000);

            CurHP = new(stats[(int)StatType.MaxHP].value.Value);
           
            Level = new(1);
            Exp = new(0);
            Gold = new(BigInteger.Parse("1000000"));


        }
        else
        {
            Level = new ReactiveProperty<int>(uData.userLevel);
            Exp = new ReactiveProperty<int>(uData.userExp);
            Gold = new ReactiveProperty<BigInteger>(BigInteger.Parse(uData.gold));
            CurHP = new ReactiveProperty<BigInteger>(BigInteger.Parse(uData.userCurHp));
            foreach(var data in uData.statData)
            {
                
           
               
                stats[(int)data.key] = new()
                {
                    value = new ReactiveProperty<BigInteger>(BigInteger.Parse(data.value)),
                    level = new ReactiveProperty<int>(data.level),
                    key = data.key,
                    textName = data.textName,
                    cost = new ReactiveProperty<BigInteger>(BigInteger.Parse(data.cost)),
                    costRate = data.costRate,
                    upgradeRate = data.upgradeRate,
                    floatScale = data.floatScale,
                    
                };
               
            }
        
        }
    
    }

    public Stat GetStat(StatType type)
    {


        return stats[(int)type];

    }

    public Stat[] GetStats()
    {
        return stats;
    }

    public void IncreaseStat(Stat stat)
    {
        int level = stat.level.Value;

        BigInteger increase;
        increase += (BigInteger)(level * stat.upgradeRate);
        stat.value.Value = increase;

        stat.level.Value += 1;
        stat.cost.Value = IncreaseCost(stat.cost.Value,level,stat.costRate);
        
    }

    public BigInteger IncreaseCost(BigInteger cost,int level,float rate)
    {   
        BigInteger increaseCost = cost * (BigInteger)Math.Pow(level,rate);

        return increaseCost;

    }
    FinalDamage fDamage= new();

    public FinalDamage Damage()
    {
        
        BigInteger damage = stats[(int)StatType.AttackDamage].value.Value;
         
        float rand = Random.value;

        if(rand < stats[(int)StatType.CriticalRate].GetFValue())
        {
            
            BigInteger criticalDamage = damage * stats[(int)StatType.CriticalDamage].value.Value;
            fDamage.damage = criticalDamage;
            fDamage.isCritical = true;

            return fDamage;
        }
        fDamage.damage = damage;
        fDamage.isCritical = false;
       
        return fDamage; 
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
    public ReactiveProperty<BigInteger> value;
    public ReactiveProperty<int> level;

    public float floatScale;

    public ReactiveProperty<BigInteger> cost;
    public float costRate;
    public float upgradeRate;

    public Stat() {}

    public Stat(BigInteger value,StatType key,string textName,BigInteger cost ,float costRate = 1,float upgradeRate = 1,int level = 1,float floatScale = 0)
    {
        this.value = new (value);
        this.key = key;
        this.textName = textName;
        this.level = new (level);
        this.cost = new(cost);
        this.floatScale = floatScale;
        this.costRate = costRate;
        this.upgradeRate = upgradeRate;
    }

    public float GetFValue()
    {
        if(floatScale == 0)
        {
           throw new InvalidOperationException("floatScale이 0이므로 GetFValue()를 호출할 수 없습니다.");
        }

        float fValue = (int)value.Value / floatScale;


        return fValue;


    }

  
  
   
}

public enum StatType
{
    MaxHP, 
    AttackRange, 
    AttackCnt, 
    AttackDamage, 
    AttackSpeed,
    AddGoldAmount, 
    StunTime, 
    StunRate, 
    CriticalRate, 
    CriticalDamage

}



    





