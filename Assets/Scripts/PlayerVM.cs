
using System.Collections.Generic;
using System.Numerics;
using R3;


// playerStats - PlayerViewModel(this) - HUD mvvm
public class PlayerVM
{
    
    View view;

    StatsSO model;

    public Stat GetStat(StatType type) => model.GetStats()[(int)type];
   
    public ReactiveProperty<BigInteger>  Gold  => model.Gold;
    
    public ReactiveProperty<BigInteger> CurHP => model.CurHP;
    // public Stat AttackRange => model.AttackRange;
    // public Stat AttackCnt => model.AttackCnt;
    // public Stat AttackDamage => model.AttackDamage;
    // public Stat AttackSpeed => model.AttackSpeed;
   
    // public Stat AddGoldAmount => model.AddGoldAmount;
    // public Stat StunTime => model.StunTime;
    // public Stat StunRate => model.StunRate;
    // public Stat CriticalRate => model.CriticalRate;
    // public Stat CriticalDamage => model.CriticalDamage;
    // public Stat MaxHP => model.MaxHP;


    public PlayerVM(View view,StatsSO model)
    {
        this.view = view;
        this.model = model;


       
    
        

       
    }

    public Stat[] GetStats()
    {
        

        return model.GetStats();

    
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

    public void BigIntStatUpgrade(Stat stat)
    {
        

        if(UseGold(stat.cost.Value))
        {
            model.IncreaseStat(stat);
        }
           


    }


    


  



    
   
}
