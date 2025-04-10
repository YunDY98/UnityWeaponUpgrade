using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Player player;

    int maxHP = 1000;

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
    }

    void Start()
    {
        HP = maxHP;
        AttackRange = 10;
        AttackCnt = 10;
        Damage = 10;
        AttackDelay = 3f;
    }

    void Update()
    {
        print(HP);
    }

   

    int _hp;
    public int HP
    {
        get { return _hp; }
        set
        { 
            _hp = value; 
            HPEvent?.Invoke((float)_hp/maxHP);
            if (_hp <= 0 && player.state != State.Die)
            {
                player.state = State.Die;
            }

            
        }
    }

    float _atkRange;
    public float AttackRange
    {
        get { return _atkRange;}
        set 
        {
            _atkRange = value;
           
        }
    }

    int _attackCnt;
    public int AttackCnt
    {
        get { return _attackCnt; }
        set { _attackCnt = value; }

    }
    float _damage;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    float _attackDelay;
    public float AttackDelay
    {
        get { return _attackDelay; }
        set { _attackDelay = value; }

    }

    int _level;
    public int LowLevel
    {
        get { return _level; }
        set { _level = value; }
    }



    
}
