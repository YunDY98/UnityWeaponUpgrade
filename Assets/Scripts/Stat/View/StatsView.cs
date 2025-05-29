using System.Collections;
using R3;
using UnityEngine;
using UnityEngine.UI;




public class StatsView : MonoBehaviour
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

        StartCoroutine(LoadStats());

       

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
        // UpgradeUI로 다운캐스팅 
        var child = item as UpgradeUI;
        // 보여줄 stat index
        var statInfo = viewModel.statInfos[rowIndex];

        child.statName.text = statInfo.statName;
        child.image.sprite = statInfo.sprite;
        child.maxLevelText.text = $"(Max:{statInfo.maxLevelText})";

        // UI 재사용을 위해 구독 해제후 새로 구독
        child.sub.Clear();

        statInfo.cost.Subscribe(x => child.cost.text = x).AddTo(child.sub);
        statInfo.level.Subscribe(x => child.levelText.text = $"Lv.{x}").AddTo(child.sub);
        statInfo.description.Subscribe(x => child.description.text = x).AddTo(child.sub);


        child.btn.onClick.RemoveAllListeners();
        // 스탯이 만렙이 아니라면
        if (statInfo.level.Value != statInfo.maxLevelText)
        {
            // 업그레이드 버튼 
            child.btn.onClick.AddListener(() => viewModel.StatUpgrade(viewModel.GetStat(statInfo.type), viewModel.statUpMult.Value));

        }

    }

    IEnumerator LoadStats()
    {
        while (!DataManager.Instance.isLoaded[(int)DataEnum.statSprite])
            yield return null;
        viewModel.SetUpgradeUI();
        theList.RowCount = viewModel.statInfos.Count;
      
    }



    public void TestGold()
    {
        viewModel.TestGold();
    }




}
