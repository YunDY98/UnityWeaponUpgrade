using System;
using UnityEditor;
using UnityEngine;

public class ItemPool : ObjectPool
{

    [SerializeField]
    RectTransform goldIcon;
    
    protected override void Create(int type)
    {

        var obj = Instantiate(objects[type],transform);

        

        obj.GetComponent<IPoolable>().ReturnEvent += Return;
        obj.GetComponent<IUITarget>().Target = goldIcon;
        

        obj.SetActive(false);

        pool[type].Enqueue(obj);



        
       
    }

    public void DropItem(int type,Vector3 pos)
    {
        if(pool[type].Count == 0)
        {
            
            Create(type);
            DropItem(type,pos);
            return;

        }

        var obj = pool[type].Dequeue();

        obj.SetActive(true);
        obj.transform.position = pos;
        obj.GetComponentInChildren<IItemMove>().Move();
       
      
    }




}

public enum ItemType
{
    Gold,




    
}