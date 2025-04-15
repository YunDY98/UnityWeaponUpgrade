using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using Assets.Scripts;
using Unity.VisualScripting;



public class View : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    TextMeshProUGUI goldText;

    [SerializeField]
    Button atkUpButton;
    [SerializeField]
    TextMeshProUGUI aktUpText;


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

        atkUpButton.onClick.AddListener(() => viewModel.UpgradeATK()); // 공격력 업그레이드 
        atkUpButton.AddComponent<LongClick>();

        viewModel.Gold.Subscribe(newGold => goldText.text = Utility.FormatNumberKoreanUnit(viewModel.Gold.Value)); // 골드 표기
    }
  


    
   
}
