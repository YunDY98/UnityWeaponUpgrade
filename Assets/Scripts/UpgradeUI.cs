using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI: MonoBehaviour
{
    public TextMeshProUGUI statName;
    public TextMeshProUGUI cost;
    public Button btn;
    public TextMeshProUGUI description;

    public TextMeshProUGUI level;
    
    public LongClick longClick;
    public Image image;

    public  CompositeDisposable sub = new();


    void Start()
    {
       longClick = btn.GetComponent<LongClick>();
    }


    void OnDisable()
    {
       
       
    }

    public void Destroy()
    {
        btn.onClick.RemoveAllListeners();
        
        
        

    }









}
