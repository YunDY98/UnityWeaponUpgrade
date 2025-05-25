using UnityEngine;
using System;


public class Gold : ItemMove, IPoolable
{
    public ItemType type;
    public StatsSO statsSO;

    public event Action<GameObject, int> OnPoolReturn;

    void Awake()
    {
        type = ItemType.Gold;

    }

    void OnDisable()
    {
        OnPoolReturn?.Invoke(gameObject, (int)type);
        statsSO.AddGold(statsSO.GetStat(StatType.AddGoldAmount).value.Value * statsSO.Level.Value);
    }




}
