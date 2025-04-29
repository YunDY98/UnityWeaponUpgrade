using System.Numerics;
using R3;
using UnityEngine;
using UnityEngine.UI;


public class PlayerVM
{


    readonly StatsSO _model;

    public Stat GetStat(int type) => _model.GetStat(type);
    public Stat GetStat(StatType type)  => _model.GetStat(type);

 
    

    public ReactiveProperty<BigInteger>  Gold  => _model.Gold;

    
    public ReactiveProperty<BigInteger> CurHP => _model.CurHP;

    public ReactiveProperty<int> Exp => _model.Exp;

    public ReactiveProperty<int> Level => _model.Level;

    public ReactiveProperty<bool> IsDead = new();

    public ReactiveProperty<int> statUpMult = new(1);


    public PlayerVM(StatsSO model)
    {
       
        _model = model;
        CurHP.Subscribe(newHP => 
        {
            IsDead.Value = newHP <= 0;

        });

        
    }

    public void SetStatUpMult(int mult)
    {
        
        statUpMult.Value = mult;
    }


    public Stat[] GetStats()
    {
        

        return _model.GetStats();

    
    }



    public bool UseGold(BigInteger useGold)
    {
        return _model.UseGold(useGold);  
    }

    public void AddGold(BigInteger gold)
    {
        _model.AddGold(gold);
    }

   
    public void StatUpgrade(Stat stat,int increase = 1)
    {
        

        if(UseGold(stat.cost.Value))
        {
            _model.IncreaseStat(stat,increase);
        }
           


    }

    public async void LoadSprite(string address,Image target)
    {
        Sprite sprite = await _model.LoadImageAsync(address);
        if (sprite != null)
        {
            target.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("이미지 없음");
        }
    }



    


    public void TestGold()
    {
        Gold.Value += BigInteger.Parse("999999999999999999999999999999");
    }


    


  



    
   
}
