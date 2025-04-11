using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    public GameObject[] objects;

    public Transform spawnPos;

    protected Queue<GameObject> pool = new();
   
    protected virtual void Create()
    {
        foreach(var obj in objects)
        {
            var tmp = Instantiate(obj,transform);
            tmp.GetComponent<T>().ReturnEvent += Return;
            tmp.SetActive(false);
            pool.Enqueue(tmp);

        }

    }

    public virtual void Return(GameObject obj)
    {
        pool.Enqueue(obj);
        obj.SetActive(false);

    }

    

   
}
public interface IPoolable
{
    event System.Action<GameObject> ReturnEvent;
}