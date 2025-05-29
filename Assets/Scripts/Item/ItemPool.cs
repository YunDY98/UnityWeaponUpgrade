using System;
using UnityEngine;

public class ItemPool : ObjectPool
{

    [SerializeField]
    Targets[] targets;

    protected override void Awake()
    {
        base.Awake();

        // 타입에 순서에 맞게 정렬 
        Array.Sort(targets, (a, b) => a.itemType.CompareTo(b.itemType));

    }
    protected override void Create(int type)
    {

        var obj = Instantiate(objects[type], transform);

        obj.GetComponent<IPoolable>().OnPoolReturn += ReturnToPool;
        obj.GetComponent<IUITarget>().Target = targets[type].target;


        obj.SetActive(false);

        pool[type].Enqueue(obj);

    }

    // 드랍할 아이템의 타입과 위치
    public void DropItem(int type, Vector3 pos)
    {

        if (pool[type].Count == 0)
        {

            Create(type);

        }

        var obj = pool[type].Dequeue();

        obj.SetActive(true);
        // 아이템 드랍 위치 (에너미가 죽은 위치)
        obj.transform.position = pos;
        // 드랍한 위치에서 타깃으로 이동 
        obj.GetComponentInChildren<IItemMove>().Move(obj.transform);


    }




}

public enum ItemType
{
    Gold,

}

[Serializable]
public struct Targets
{
    public ItemType itemType;
    public RectTransform target;
}

