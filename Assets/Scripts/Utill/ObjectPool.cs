using System;
using System.Collections.Generic;

using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public GameObject[] objects;


    public Transform spawnPos;

    protected Queue<GameObject>[] pool;


    // 오브젝트를 생성하여 타입(골드, 아이템 등)에 맞는 풀에 추가 
    protected abstract void Create(int type);

    // 사용 후 풀로 반환
    public virtual void ReturnToPool(GameObject obj, int type)
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
        for (int i = 0; i < objects.Length; ++i)
        {
            pool[i] = new Queue<GameObject>();
        }


    }

}
public interface IPoolable
{
    event Action<GameObject, int> OnPoolReturn;

}

