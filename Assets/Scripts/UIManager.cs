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


        viewModel.CurHP.Subscribe(newHP => 
        {
           
            if(newHP <= 0)
            {
                deathUI.SetActive(true);

            }

        });
       
        
    }

    void Penalty()
    {
        deathUI.SetActive(false);
        viewModel.Level.Value -= 1;
        player.Init();
        viewModel.CurHP.Value = viewModel.GetStat(StatType.MaxHP).value.Value;

    }

}
