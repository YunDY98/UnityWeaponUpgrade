using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  
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
    }

    void Start()
    {
        HP = 1000;
    }

    void Update()
    {
        print(HP);
    }

    Player player;

    int _hp;
    public int HP
    {
        get { return _hp; }
        set
        { 
            _hp = value; 
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
        set { _atkRange = value; }
    }

    int _attackCnt;
    public int AttackCnt
    {
        get { return _attackCnt; }
        set { _attackCnt = value; }

    }

    int _level;
    public int LowLevel
    {
        get { return _level; }
        set { _level = value; }
    }



    public void Player(Player player)
    {
        this.player = player;
    }
}
