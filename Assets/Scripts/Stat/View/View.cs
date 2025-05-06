using UnityEngine;
using UnityEngine.UI;




public class View : MonoBehaviour
{

    [SerializeField]
    UpgradeUI uObject;




    [SerializeField]
    Button[] multBtn;





    [SerializeField]
    RectTransform uContent;

    StatsVM viewModel;



    void Start()
    {
        viewModel = GameManager.Instance.statsVM;



        if (multBtn[0] == null) return;
        multBtn[0].onClick.AddListener(() => StatLevelUpMult(1, multBtn[0]));
        StatLevelUpMult(1, multBtn[0]);
        multBtn[1].onClick.AddListener(() => StatLevelUpMult(10, multBtn[1]));
        multBtn[2].onClick.AddListener(() => StatLevelUpMult(100, multBtn[2]));

        CreateUpgradeUI();
       


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            viewModel.ShowUpgradeUI(1, 1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            viewModel.ShowUpgradeUI(1, 3);
            viewModel.ShowUpgradeUI(3, 1);

        }
    }

    void StatLevelUpMult(int x, Button btn)
    {
        viewModel.SetStatUpMult(x);
        foreach (var select in multBtn)
        {
            ColorBlock cb = select.colors;
            if (select == btn)
            {

                cb.normalColor = Color.black;
                cb.selectedColor = Color.black;

            }
            else
            {
                cb.normalColor = Color.white;

            }
            select.colors = cb;
        }

    }

    void CreateUpgradeUI()
    {
        for (int i = 0; i < 6; ++i)
        {
            var obj = Instantiate(uObject, uContent);
            var ui = obj.GetComponent<UpgradeUI>();

            viewModel.showUIList.Add(ui);


        }

    }




   

    public void TestGold()
    {
        viewModel.TestGold();
    }





}
