using DG.Tweening;
using UnityEngine;

public class TouchEffect : ObjectPool
{
    bool isClick = false;
   
    Vector2 clickPos;
    protected override void Awake()
    {
    
        Init(); 
        
    }

    protected override void Create(int type = 0)
    {
        
        GameObject obj = Instantiate(objects[0]);
        obj.SetActive(false);
        obj.transform.SetParent(spawnPos,false);

        pool[0].Enqueue(obj);

    }

    void Update()
    {
        #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
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

    public void TouchRing(Vector2 pos)
    {
        
       
        if(pool[0].Count == 0)
        {
            Create(0);
            
        }
       
           
        var obj = pool[0].Dequeue();
     
    
        
       
        obj.transform.position = pos;
        obj.SetActive(true);

        TouchAnim(obj);
       
    }

    public void TouchAnim(GameObject obj)
    {
            // 처음에 스케일을 0으로 시작
        obj.transform.localScale = Vector3.zero;

        // DOTween을 사용하여 스케일을 1까지 확대
        obj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnKill(() => Return(obj, 0));

    }
 
}
