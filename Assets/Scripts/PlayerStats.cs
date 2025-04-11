using System;
using NUnit.Framework.Constraints;
using R3;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [HideInInspector]
    public Player player;


    

    public event Action<float> HPEvent;
    private static PlayerStats _instance;
    public static PlayerStats Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);


        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        player = GetComponent<Player>();

       
        HP.Value = MaxHP.Value;
      
    }

    void Update()
    {
        print(HP);
    }

   
    public ReactiveProperty<int> MaxHP = new(1000);
    public ReactiveProperty<int> HP = new ();
    public ReactiveProperty<float> AttackRange = new (10);
    public ReactiveProperty<int> AttackCnt = new (10);
    public ReactiveProperty<float> Damage = new (1000);
    public ReactiveProperty<float> AttackDelay = new (3f);
    public ReactiveProperty<int> Level = new (0);



    
}
