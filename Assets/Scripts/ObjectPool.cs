using System;
using System.Collections.Generic;

using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public GameObject[] objects;
    
    [HideInInspector]
    public Transform spawnPos;

    protected Queue<GameObject>[] pool;
   
    protected abstract void Create(int type);
    

    public virtual void Return(GameObject obj,int type)
    {
        pool[type].Enqueue(obj);
        obj.SetActive(false);

    }

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        pool = new Queue<GameObject>[objects.Length];
        for(int i=0; i<objects.Length; ++i)
        {
            pool[i] = new Queue<GameObject>();


    
        }
   
        
    }




}
public interface IPoolable
{
    event Action<GameObject,int> ReturnEvent;
    
}

