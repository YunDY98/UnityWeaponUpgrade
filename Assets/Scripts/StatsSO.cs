using System.Numerics;
using R3;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO", menuName = "ScriptableObjects/Player", order = 1)]

public class StatsSO : ScriptableObject
{
  
    public ReactiveProperty<BigInteger> MaxHP = new(3000);
    public ReactiveProperty<BigInteger> HP = new ();
    public ReactiveProperty<float> AttackRange = new (10);
    public ReactiveProperty<int> AttackCnt = new (10);
    public ReactiveProperty<BigInteger> AttackDamage = new (10);
    public ReactiveProperty<float> AttackDelay = new (3f);
    public ReactiveProperty<int> Level = new (1);
    public ReactiveProperty<BigInteger> Gold = new (BigInteger.Parse("190635600001000"));
    public ReactiveProperty<BigInteger> AddGoldAmount = new (BigInteger.Parse("1000000000000000")); // 골드 획등량 
    public ReactiveProperty<float> StunTime = new();
    public ReactiveProperty<float> StunRate = new();
    public ReactiveProperty<float> CriticalRate = new(0.2f);
    public ReactiveProperty<int> CriticalDamage = new(2);

   
    FinalDamage fDamage= new();
    public void IncreaseATK()
    {
        AttackDamage.Value += 10;
    }

    public FinalDamage Damage()
    {
        
        BigInteger damage = AttackDamage.Value;
         
        float rand = Random.value;

        if(rand < CriticalRate.Value)
        {
            
            BigInteger criticalDamage = damage * CriticalDamage.Value;
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