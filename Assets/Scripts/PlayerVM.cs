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


    public void UseGold(int useGold)
    {
        model.Gold.Value -= useGold;
    }


  



    
   
}
