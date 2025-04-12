using System;
using NUnit.Framework.Constraints;
using R3;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO", menuName = "ScriptableObjects/Player", order = 1)]

public class StatsSO : ScriptableObject
{

    public event Action<float> HPEvent;

   
    public ReactiveProperty<int> MaxHP = new(1000);
    public ReactiveProperty<int> HP = new ();
    public ReactiveProperty<float> AttackRange = new (10);
    public ReactiveProperty<int> AttackCnt = new (10);
    public ReactiveProperty<float> Damage = new (1000);
    public ReactiveProperty<float> AttackDelay = new (3f);
    public ReactiveProperty<int> Level = new (0);



    
}
