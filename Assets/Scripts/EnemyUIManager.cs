using UnityEngine;
using DG.Tweening;
using TMPro;
using Assets.Scripts;

public class EnemyUIManager : ObjectPool
{

    private static EnemyUIManager _instance;
    public static EnemyUIManager  Instance
    {
        get{ return _instance;}


    }

    [SerializeField]
    Transform canvas;

    
    

    float duration = 2f;



    protected override void Awake()
    {

        if (_instance != null)
        {
           Destroy(gameObject);
        }
        else
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        Init();
    }

    protected override void Create(int type = 0)
    {

        GameObject obj = Instantiate(objects[type],canvas);

        obj.SetActive(false);
        pool[type].Enqueue(obj);

    }


    public void Damage(Vector3 pos,FinalDamage fDamage,int type = 0)
    {
        if(pool[type].Count == 0)
        {
            Create(type);

        }
           
        var obj = pool[type].Dequeue();
    
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        obj.transform.position = screenPos;
        var tmp = obj.GetComponent<TextMeshProUGUI>();

        if(fDamage.isCritical)
        {
            tmp.color = Color.red;
        }
        else
        {
            tmp.color = Color.yellow;
        }

        tmp.text = Utility.FormatNumberKoreanUnit(fDamage.damage);
        tmp.alpha = 1;
        
        obj.SetActive(true);
        

        obj.transform.DOMoveY(obj.transform.position.y + 30f, duration)
            .SetEase(Ease.OutCubic)
            .OnKill(() => Return(obj, type));

        tmp.DOFade(0, duration).SetEase(Ease.InOutQuad);



        


        
        
    }

    void OnDestroy()
    {
        DOTween.KillAll(); // 모든 트윈 애니메이션 종료
        
    }





}
