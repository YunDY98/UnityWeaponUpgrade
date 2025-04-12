using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using System.Numerics;
using UnityEngine.EventSystems;
using System.Collections;
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

        viewModel.Gold.Subscribe(newGold => goldText.text = ToKoreanFormat(viewModel.Gold.Value)); // 골드 표기
    }

   
   

    public string ToKoreanFormat(BigInteger value)
    {
        var result = "";

        int cnt = 0;

        BigInteger d = 1_0000_0000_0000_0000;
        BigInteger c = 1_0000_0000_0000;
        BigInteger b = 1_0000_0000;
        BigInteger a = 10_000;

        if (value >= d && cnt < 2)
        {
            cnt++;
            result += $"{value / d}경 ";
            value %= d;
           
            
        }
        if (value >= c && cnt < 2)
        {
            cnt++;
            result += $"{value / c}조 ";
            value %= c;
        }
        if (value >= b && cnt < 2)
        {
            cnt++;
            result += $"{value / b}억 ";
            value %= b;
            
        }
        if (value >= a && cnt < 2)
        {
            cnt++;
            result += $"{value / a}만 ";
            value %= a;
        }
        if (value > 0 && cnt < 2)
        {
            cnt++;
            result += $"{value}";
        }

        return string.IsNullOrEmpty(result) ? "0" : result.Trim();
    }


    
   
}
