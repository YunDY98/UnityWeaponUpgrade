using UnityEngine;
using System;


public class Gold : ItemMove,IPoolable
{
    public ItemType type;
    public StatsSO statsSO;

    public event Action<GameObject,int> ReturnEvent;

    void Awake()
    {
        type = ItemType.Gold;

    }

    void OnDisable()
    {
        ReturnEvent?.Invoke(gameObject,(int)type);
        statsSO.Gold.Value += statsSO.GetStat(StatType.AddGoldAmount).value.Value * statsSO.Level.Value;
    }





}
