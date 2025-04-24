using UnityEngine;
using DG.Tweening;
using TMPro;
using Assets.Scripts;
using Unity.Multiplayer.Center.Common;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections;

public class EffectManager : ObjectPool,ICanvasRaycastFilter
{

   
    
    RectTransform rectTransform;
    bool isTouch = true;
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
       
        if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera))
        {
            
            if(isTouch)
            {
                TouchRing(sp);
                StartCoroutine(Touch());

            }
            
            
         
            return false; 
        }


        
        return true;
    }


    float duration = 2f;



    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        

        Init();
        
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
            Create();

        
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
    void OnDestroy()
    {
        DOTween.KillAll(); // 모든 트윈 애니메이션 종료
        
    }

    IEnumerator Touch()
    {
        isTouch = false;
        yield return new WaitForFixedUpdate();
        isTouch = true;
    }





}

public enum Effect
{
    Damage,
    TouchRing,
}

