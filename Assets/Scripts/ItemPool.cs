
using System.Collections.Generic;

using UnityEngine;

public class ItemPool : ObjectPool
{

    [SerializeField]
    RectTransform[] icon; // ItemType Enum과 인스펙터 배치 순서가 맞아야함 

    protected override void Create(int type)
    {

        var obj = Instantiate(objects[type],transform);

        

        obj.GetComponent<IPoolable>().ReturnEvent += Return;
        obj.GetComponent<IUITarget>().Target = icon[type];
        

        obj.SetActive(false);

        pool[type].Enqueue(obj);



        
       
    }

    public void DropItem(int type,Vector3 pos)
    {
        if(pool[type].Count == 0)
        {
            
            Create(type);

        }

        var obj = pool[type].Dequeue();

        obj.SetActive(true);
        obj.transform.position = pos;
        obj.GetComponentInChildren<IItemMove>().Move(obj.transform);
       
      
    }




}

public enum ItemType
{
    Gold,




    
}

