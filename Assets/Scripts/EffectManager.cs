using UnityEngine;
using DG.Tweening;
using TMPro;
using Assets.Scripts;

public class EffectManager : ObjectPool
{

   
    

   

    float duration = 2f;

   
    bool isClick = false;
   
    Vector2 clickPos;

    protected override void Awake()
    {
        
        

        Init();
        
        
    }

    void Update()
    {
        #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            print(1);
            clickPos = Input.mousePosition;
            if(!isClick)
                TouchRing(clickPos);
           
           isClick = true;
        }
        if(Input.GetMouseButtonUp(0))
            isClick = false;
        #endif


        #if UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
             
          
            if(touch.phase == TouchPhase.Began)
            {
                clickPos = touch.position;
                TouchRing(clickPos);
            }
           
           
        }
        #endif
       


        
    }
    void OnDestroy()
    {
        DOTween.KillAll(); // 모든 트윈 애니메이션 종료
        
    }


    protected override void Create(int type = 0)
    {
        
        GameObject obj = Instantiate(objects[type]);
        obj.SetActive(false);
        obj.transform.SetParent(spawnPos,false);

        pool[type].Enqueue(obj);

    }


    public void Damage(Vector3 pos,FinalDamage fDamage)
    {
        int type = (int)Effect.Damage;
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

    public void TouchRing(Vector2 pos)
    {
        
        int type = (int)Effect.TouchRing;
        if(pool[type].Count == 0)
        {
            Create(type);
            
        }
       
           
        var obj = pool[type].Dequeue();
     
    
        
       
        obj.transform.position = pos;
        obj.SetActive(true);

        TouchAnim(obj);
       
        
        
        

    }

    public void TouchAnim(GameObject obj)
    {
            // 처음에 스케일을 0으로 시작
        obj.transform.localScale = Vector3.zero;

        // DOTween을 사용하여 스케일을 1까지 확대
        obj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnKill(() => Return(obj, (int)Effect.TouchRing));

    }
 

    

  

}

public enum Effect
{
    Damage,
    TouchRing,
}

