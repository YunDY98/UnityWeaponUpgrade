
using System.Collections.Generic;
using System.Numerics;
using R3;


public class PlayerVM
{


    StatsSO model;

    public Stat GetStat(StatType type) => model.GetStats()[(int)type];
   
    public ReactiveProperty<BigInteger>  Gold  => model.Gold;
    
    public ReactiveProperty<BigInteger> CurHP => model.CurHP;

    public ReactiveProperty<int> Level => model.Level;


    public PlayerVM(StatsSO model)
    {
       
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
