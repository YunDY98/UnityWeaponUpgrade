using UnityEngine;
using UnityEngine.UI;
using R3;
public class UIManager : MonoBehaviour
{
    
    [SerializeField]
    GameObject deathUI;

    [SerializeField]
    Player player;

    PlayerVM viewModel;
    

    void Start()
    {
        viewModel = GameManager.Instance.playerVM;

        Button penalty = deathUI.GetComponentsInChildren<Button>()[0];
       

        penalty.onClick.AddListener(() => Penalty());


        viewModel.IsDead.Subscribe(dead => deathUI.SetActive(dead));
       
        
    }

    void Penalty()
    {
        deathUI.SetActive(false);
        viewModel.Exp.Value = 0;
        player.Init();
        viewModel.CurHP.Value = viewModel.GetStat(StatType.MaxHP).value.Value;

    }

}
