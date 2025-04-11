using R3;
using UnityEngine;

// playerStats - PlayerViewModel(this) - HUD mvvm
public class PlayerVM
{
    
    private HUD hud;

    private PlayerStats model;

    public ReactiveProperty<float>  HP {get; private set;} = new();

    public PlayerVM(HUD hud)
    {
        this.hud = hud;
        model = PlayerStats.Instance;

    }


  



    
   
}
