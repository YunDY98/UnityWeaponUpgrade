using UnityEngine;
using UnityEngine.UI;
using R3;
public class UIManager : MonoBehaviour
{
    
    [SerializeField]
    GameObject deathUI;

    [SerializeField]
    Player player;

    StatsVM viewModel;
    

    void Start()
    {
        viewModel = GameManager.Instance.statsVM;

        Button penalty = deathUI.GetComponentsInChildren<Button>()[0];
        Button ad = deathUI.GetComponentsInChildren<Button>()[1];
       

        penalty.onClick.AddListener(() =>
        {
            Penalty();
            Resurrction();

        } );

        ad.onClick.AddListener(() =>
        {
            Resurrction();
            WatchAD();
        });


        viewModel.IsDead.Subscribe(dead => deathUI.SetActive(dead));
       
        
    }

    void Penalty()
    {
        
        viewModel.Exp.Value = 0;
        

    }

    void WatchAD()
    {
        deathUI.SetActive(false);
        player.Init();
        viewModel.AddGold(10000);

    }

    void Resurrction()
    {
        deathUI.SetActive(false);
        player.Init();
        viewModel.CurHP.Value = viewModel.GetStat(StatType.MaxHP).value.Value;
        
    }

}
