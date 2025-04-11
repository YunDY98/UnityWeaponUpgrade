using UnityEngine;
using UnityEngine.UI;
using R3;
using NUnit.Framework.Constraints;
using UnityEditor.Rendering.Universal;

public class HUD : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    private PlayerVM viewModel;
    private PlayerStats model;

    void Awake()
    {
        viewModel = new(this);
    }

    void Start()
    { 
        
       model = PlayerStats.Instance;

       model.HP.Subscribe(newHP => 
       {
            hpSlider.value = (float)newHP / model.MaxHP.Value;
            if(newHP <= 0)
            {
                model.player.state = State.Die;
            }

       });


       
    }


   


    
   
}
