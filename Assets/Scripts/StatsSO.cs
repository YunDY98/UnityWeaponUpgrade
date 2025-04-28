using System;
using System.Numerics;
using System.Threading.Tasks;
using Assets.Scripts;
using R3;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.ResourceManagement.AsyncOperations;
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

            stats[(int)StatType.AttackDamage] = new()
            {
                value = new(1000),
                baseValue = 1000,
                key = StatType.AttackDamage,
                textName = "공격력",
                baseCost = 1000,
                cost = new(1000),
                costRate = 1.001f,
                upgradeRate = 1.001f,
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
                costRate = 1.0001f, //1.0001f, 100씩 업 기준 9999해까지 약 40만렙 

                upgradeRate = 1,
                level = new(1),
                maxLevel = 10000,
                floatScale = 0
            };

            stats[(int)StatType.AttackRange] = new()
            {
                value = new(10000),
                baseValue = 10000,
                key = StatType.AttackRange,
                textName = "공격 범위",
                baseCost = 1000,
                cost = new(BigInteger.Parse("1000000000")),
                costRate = 6,
                upgradeRate = 1,
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
                baseCost = 1000000000,
                cost = new(BigInteger.Parse("1000000000")),
                costRate = 10,
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
                baseCost = 10,
                cost = new(BigInteger.Parse("10")),
                costRate = 1.05f,
                upgradeRate = 1.01f,
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
                baseCost = 1000,
                cost = new(BigInteger.Parse("1000000")),
                costRate = 1.03f,
                upgradeRate = 1,
                level = new(1),
                maxLevel = 100,
                floatScale = 1000
            };

            stats[(int)StatType.StunRate] = new()
            {
                value = new(1),
                baseValue = 1,
                key = StatType.StunRate,
                textName = "스턴 확률",
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
                baseValue = 1,
                key = StatType.CriticalRate,
                textName = "크리티컬 확률",
                baseCost = 1000000,
                cost = new(BigInteger.Parse("1000000")),
                costRate = 1.03f,
                upgradeRate = 1f,
                level = new(1),
                maxLevel = 1000,
                floatScale = 1000
            };

            stats[(int)StatType.CriticalDamage] = new()
            {
                value = new(101),
                baseValue = 101,
                key = StatType.CriticalDamage,
                textName = "크리티컬 데미지",
                baseCost = 1000,
                cost = new(BigInteger.Parse("100000")),
                costRate = 2f,
                upgradeRate = 1.05f,
                level = new(1),
                maxLevel = 1000,
                floatScale = 1000
            };

            

            CurHP = new(stats[(int)StatType.MaxHP].baseValue);
           
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
                    baseValue = data.baseValue,
                    
                    level = new ReactiveProperty<int>(data.level),
                    key = data.key,
                    textName = data.textName,
                    baseCost = data.baseCost,
                    costRate = data.costRate,
                    upgradeRate = data.upgradeRate,
                    floatScale = data.floatScale,
                    maxLevel = data.maxLevel,

                    cost = new (),
                    value = new (),
                
                    
                };
                Stat stat = stats[(int)data.key];
                stat.cost.Value = Utility.GeoProgression(stat.baseCost,stat.costRate,stat.level.Value);
                stat.value.Value = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,stat.level.Value);
               
            }
        
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

    public void IncreaseStat(Stat stat,int increase = 1)
    {
       

        stat.LevelUp(increase);
        
        stat.value.Value = Utility.GeoProgression(stat.baseValue,stat.upgradeRate,stat.level.Value);

        
    }

       FinalDamage fDamage= new();

    public FinalDamage Damage()
    {
        
        BigInteger damage = stats[(int)StatType.AttackDamage].value.Value;
        Stat cDamage = stats[(int)StatType.CriticalDamage];
         
        double rand = Random.value;

        Debug.Log(rand +"  :" +stats[(int)StatType.CriticalRate].GetFValue() );
        if(rand < stats[(int)StatType.CriticalRate].GetFValue())
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
       
        while(Exp.Value >= Level.Value)
        {
            Exp.Value -= Level.Value;
            Level.Value++;


        }

        

        


    }

    public void AddExp(int exp)
    {
       
        Exp.Value += exp;
        if(Exp.Value >= Level.Value)
            LevelUp();

        DataManager.Instance.SaveData();
    }

    public async Task<Sprite> LoadImageAsync(string address)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        return null;
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

    public void LevelUp(int increase)
    {

        level.Value += increase;
        if(level.Value > maxLevel)
        {
            level.Value = maxLevel;
        }
        
        
    }
   
    public int maxLevel;

    public int floatScale;

    public int baseValue;

    

    public int baseCost;
    public ReactiveProperty<BigInteger> cost;
    public float costRate;
    public float upgradeRate;

    public Stat() {}

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
        if(floatScale == 0)
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
    CriticalDamage

}



    





