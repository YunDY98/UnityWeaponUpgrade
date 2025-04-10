using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;

    void Start()
    {
        PlayerStats.Instance.HPEvent += UpdateHp;   
    }

    void UpdateHp(float percent)
    {
        hpSlider.value = percent;
    }
   
}
