using UnityEngine;
using UnityEngine.UI;
using R3;

public class View : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    PlayerVM viewModel;
    [SerializeField]
    StatsSO model;

    void Awake()
    {
        viewModel = new(this,model);
        model.HP.Subscribe(newHP => 
        {
            hpSlider.value = (float)((double)newHP / (double)model.MaxHP.Value);

        });
    }

   


    
   
}
