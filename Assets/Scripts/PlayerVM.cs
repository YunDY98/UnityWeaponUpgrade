using R3;
using UnityEngine;

// playerStats - PlayerViewModel(this) - HUD mvvm
public class PlayerVM
{
    
    private HUD hud;

    StatsSO model;

    public ReactiveProperty<float>  HP {get; private set;} = new();

    public PlayerVM(HUD hud,StatsSO model)
    {
        this.hud = hud;
        this.model = model;

    }


  



    
   
}
