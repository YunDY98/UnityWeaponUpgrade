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
    [SerializeField]
    StatsSO model;

    void Awake()
    {
        viewModel = new(this,model);
    }

    void Start()
    { 

       model.HP.Subscribe(newHP => 
       {
            hpSlider.value = (float)newHP / model.MaxHP.Value;

       });


       
    }


   


    
   
}
