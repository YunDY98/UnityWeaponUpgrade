using System;

using System.Numerics;
using NUnit.Framework;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "StatsSO", menuName = "ScriptableObjects/Player", order = 1)]

public class StatsSO : ScriptableObject
{
   
   

    public Stat<BigInteger> MaxHP = new(3000,10,1);
    public ReactiveProperty<BigInteger> CurHP = new ();
    public Stat<float> AttackRange = new (10,1,1);
    public Stat<int> AttackCnt = new (10,10000000000,1);
    public Stat<BigInteger> AttackDamage = new (10,1,1);
    public Stat<float> AttackSpeed = new (3f,10000000,1);
    public ReactiveProperty<int> PlayerLevel = new (1);
    public ReactiveProperty<BigInteger> Gold = new (BigInteger.Parse("190635600001000"));
    public Stat<BigInteger> AddGoldAmount = new (BigInteger.Parse("1000000000000000"),1,100000); // 골드 획득량 
    public Stat<float> StunTime = new(0.1f,100000000000,1);
    public Stat<float> StunRate = new(0.1f,100000000,1);
    public Stat<float> CriticalRate = new(1f,100000000,1);
    public Stat<int> CriticalDamage = new(2,100000000,1);

    public PlayerStats playerStats;

    public void Init()
    {
        playerStats = new PlayerStats(MaxHP,CurHP,AttackRange,AttackCnt,AttackDamage,AttackSpeed,PlayerLevel,Gold,AddGoldAmount,StunTime,StunRate,CriticalRate,CriticalDamage);
    
    }
    public void IncreaseStat(Stat<BigInteger> stat)
    {
        int level = stat.level.Value;
        BigInteger increase = (BigInteger)Math.Pow(level * stat.upgradeRate,level / 10);
        stat.value.Value += increase;
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
        
        BigInteger damage = AttackDamage.value.Value;
         
        float rand = Random.value;

        if(rand < CriticalRate.value.Value)
        {
            
            BigInteger criticalDamage = damage * CriticalDamage.value.Value;
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

[System.Serializable]
public class Stat<T>  
{
    public string name;
    public string textName;
    public ReactiveProperty<T> value;
    public ReactiveProperty<int> level;

    public ReactiveProperty<BigInteger> cost;
    public float costRate;
    public float upgradeRate;


    public Stat(T value,BigInteger cost ,int level = 1)
    {
        this.value = new (value);
        this.level = new (level);
        this.cost = new(cost);
        
    }
  
   
}
[System.Serializable]
public class PlayerStats
{
    public Stat<BigInteger> MaxHP;
    public ReactiveProperty<BigInteger> CurHP;
    public Stat<float> AttackRange; 
    public Stat<int> AttackCnt;
    public Stat<BigInteger> AttackDamage;
    public Stat<float> AttackSpeed;
    public ReactiveProperty<int> PlayerLevel;
    public ReactiveProperty<BigInteger> Gold;
    public Stat<BigInteger> AddGoldAmount; // 골드 획득량 
    public Stat<float> StunTime;
    public Stat<float> StunRate;
    public Stat<float> CriticalRate;
    public Stat<int> CriticalDamage;
    public PlayerStats(
        Stat<BigInteger> maxHP,
        ReactiveProperty<BigInteger> curHP,
        Stat<float> attackRange,
        Stat<int> attackCnt,
        Stat<BigInteger> attackDamage,
        Stat<float> attackSpeed,
        ReactiveProperty<int> playerLevel,
        ReactiveProperty<BigInteger> gold,
        Stat<BigInteger> addGoldAmount,
        Stat<float> stunTime,
        Stat<float> stunRate,
        Stat<float> criticalRate,
        Stat<int> criticalDamage
    )
    {
        MaxHP = maxHP;
        CurHP = curHP;
        AttackRange = attackRange;
        AttackCnt = attackCnt;
        AttackDamage = attackDamage;
        AttackSpeed = attackSpeed;
        PlayerLevel = playerLevel;
        Gold = gold;
        AddGoldAmount = addGoldAmount;
        StunTime = stunTime;
        StunRate = stunRate;
        CriticalRate = criticalRate;
        CriticalDamage = criticalDamage;
    }




    

}



