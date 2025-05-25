using System;
using UnityEngine;

public class ItemPool : ObjectPool
{

    [SerializeField]
    Targets[] targets;

    protected override void Awake()
    {
        base.Awake();
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
        obj.transform.position = pos;
        obj.GetComponentInChildren<IItemMove>().Move(obj.transform);


    }




}

public enum ItemType
{
    Gold,

}

[System.Serializable]
public struct Targets
{
    public ItemType itemType;
    public RectTransform target;
}

