
using System.Collections;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UI;




public class View : MonoBehaviour
{

    public RecyclingListView theList;



    [SerializeField]
    UpgradeUI uObject;


    [SerializeField]
    ScrollRect scrollRect;


    public int rowCount = 6;
    public float RowPadding = 10f;
    protected const int rowsAboveBelow = 1;



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
        multBtn[1].onClick.AddListener(() => StatLevelUpMult(10, multBtn[1]));
        multBtn[2].onClick.AddListener(() => StatLevelUpMult(100, multBtn[2]));
        StatLevelUpMult(1, multBtn[0]);

        theList.ItemCallback = PopulateItem;

        StartCoroutine(WaitForLoading());

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


    private void PopulateItem(RecyclingListViewItem item, int rowIndex)
    {

        var child = item as UpgradeUI;
        var data = viewModel.datas[rowIndex];
        child.statName.text = data.statName;
        child.image.sprite = data.sprite;

        child.sub.Clear();

        data.cost.Subscribe(x => child.cost.text = x).AddTo(child.sub);
        data.level.Subscribe(x => child.level.text = x).AddTo(child.sub);

        child.maxLevelText.text = data.maxLevelText;

        data.description.Subscribe(x => child.description.text = x).AddTo(child.sub);


        child.btn.onClick.RemoveAllListeners();

        if (data.level.Value != data.maxLevelText)
        {
            child.btn.onClick.AddListener(() => viewModel.StatUpgrade(viewModel.GetStat(data.type), viewModel.statUpMult.Value));

        }

    }


    IEnumerator WaitForLoading()
    {

        while (!viewModel.datas.All(d => d.sprite != null))
        {

            yield return null;
        }
        
        theList.RowCount = viewModel.datas.Count;
        Loading.Instance.currentLoadCnt += Loading.Instance.spriteLoadCnt;
       

    }


    public void TestGold()
    {
        viewModel.TestGold();
    }


}
