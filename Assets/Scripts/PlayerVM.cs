using System.Collections.Generic;
using System.Numerics;
using R3;


// playerStats - PlayerViewModel(this) - HUD mvvm
public class PlayerVM
{
    
    View view;

    StatsSO model;

    public ReactiveProperty<BigInteger>  Gold  => model.Gold;
    public Stat<BigInteger> MaxHP => model.MaxHP;
    public ReactiveProperty<BigInteger> CurHP => model.CurHP;
    public Stat<float> AttackRange => model.AttackRange;
    public Stat<int> AttackCnt => model.AttackCnt;
    public Stat<BigInteger> AttackDamage => model.AttackDamage;
    public Stat<float> AttackSpeed => model.AttackSpeed;
    public ReactiveProperty<int> PlayerLevel => model.PlayerLevel;
    public Stat<BigInteger> AddGoldAmount => model.AddGoldAmount;  
    public Stat<float> StunTime => model.StunTime;
    public Stat<float> StunRate => model.StunRate;
    public Stat<float> CriticalRate => model.CriticalRate;
    public Stat<int> CriticalDamage => model.CriticalDamage;

    public Dictionary<string,Stat<BigInteger>> BigIntStatDic = new();


    public PlayerVM(View view,StatsSO model)
    {
        this.view = view;
        this.model = model;

        BigIntStatDic.Add("MaxHP",MaxHP);
        BigIntStatDic.Add("AttackDamage",AttackDamage);
        BigIntStatDic.Add("AddGoldAmount",AddGoldAmount);
    }


    public bool UseGold(BigInteger useGold)
    {
        

        if(Gold.Value - useGold < 0)
        {
            return false;
        }
        Gold.Value -= useGold;
        

        return true;
        
        
    }

    public Stat<BigInteger> GetAttackDamage()
    {
        return model.AttackDamage;
    }

    public void BigIntStatUpgrade(KeyValuePair<string,Stat<BigInteger>> dic)
    {
        

        if(UseGold(dic.Value.cost.Value))
        {
            model.IncreaseStat(dic.Value);
        }
           


    }


    


  



    
   
}
