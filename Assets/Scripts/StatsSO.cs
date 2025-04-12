using System;
using System.Numerics;
using NUnit.Framework.Constraints;
using R3;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSO", menuName = "ScriptableObjects/Player", order = 1)]

public class StatsSO : ScriptableObject
{
  
    public ReactiveProperty<BigInteger> MaxHP = new(1000);
    public ReactiveProperty<BigInteger> HP = new ();
    public ReactiveProperty<float> AttackRange = new (10);
    public ReactiveProperty<int> AttackCnt = new (10);
    public ReactiveProperty<BigInteger> Damage = new (1000);
    public ReactiveProperty<float> AttackDelay = new (3f);
    public ReactiveProperty<int> Level = new (0);
    public ReactiveProperty<BigInteger> Gold = new (0);
    public ReactiveProperty<float> StunTime = new();
    public ReactiveProperty<float> StunRate = new();
    public ReactiveProperty<float> CriticalRate = new();
    public ReactiveProperty<float> CriticalDamage = new();



    
}
