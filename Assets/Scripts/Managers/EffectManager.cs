using UnityEngine;
using DG.Tweening;
using TMPro;
using Assets.Scripts;

public class EffectManager : ObjectPool
{
    float duration = 2f;

   
   

    protected override void Awake()
    {
    
        Init(); 
        
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

    

    

  

}

public enum Effect
{
    Damage,
}

