using System.Numerics;
using R3;


public class PlayerVM
{


    StatsSO model;

    public Stat GetStat(StatType type) => model.GetStats()[(int)type];
   
    public ReactiveProperty<BigInteger>  Gold  => model.Gold;
    
    public ReactiveProperty<BigInteger> CurHP => model.CurHP;

    public ReactiveProperty<int> Exp => model.Exp;

    public ReactiveProperty<int> Level => model.Level;

    public ReactiveProperty<bool> IsDead = new();

     public ReactiveProperty<int> statLevelUpMult = new(1);


    public PlayerVM(StatsSO model)
    {
       
        this.model = model;
        CurHP.Subscribe(newHP => 
        {
            IsDead.Value = newHP <= 0;
        });

       
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

    public void StatUpgrade(Stat stat,int increase = 1)
    {
        

        if(UseGold(stat.cost.Value))
        {
            model.IncreaseStat(stat,increase);
        }
           


    }


    public void TestGold()
    {
        Gold.Value += BigInteger.Parse("99999999999999999999999");
    }


    


  



    
   
}
