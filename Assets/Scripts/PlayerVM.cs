using System.Numerics;
using R3;
using UnityEditor;
using UnityEngine;

// playerStats - PlayerViewModel(this) - HUD mvvm
public class PlayerVM
{
    
    View view;

    StatsSO model;

    public ReactiveProperty<BigInteger>  Gold  => model.Gold;

    public PlayerVM(View view,StatsSO model)
    {
        this.view = view;
        this.model = model;

    }


    public bool UseGold(BigInteger useGold)
    {
        

        if(model.Gold.Value - useGold < 0)
        {
            return false;
        }
        model.Gold.Value -= useGold;
        

        return true;
        
        
    }

    public void UpgradeATK()
    {

        if(UseGold(10000000000000))
            model.IncreaseATK();


    }


  



    
   
}
