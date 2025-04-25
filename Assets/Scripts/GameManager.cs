using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isLive;
    float time;
    float spawnTime = 2f;
    int _enemyCnt;

    int enemySort;
    public int EnemyCnt
    {
        get { return _enemyCnt; }
        set { _enemyCnt = value; }
    }

    int spawnCnt = 5;
    public EnemyFactory EnemyFactory;

    public StatsSO statsSO;

    [HideInInspector]
    public PlayerVM playerVM;

    bool _stop = false;

    public bool Stop
    {
        get => _stop;
        set
        {
            _stop = value;
        }
    }



    private static GameManager _instance;
    public static GameManager Instance
    {
        get{ return _instance;}


    }
    private bool _isMove = true;

    public bool IsMove
    {
        get{ return _isMove;}
        set{ _isMove = value; }
    }

  

    void Awake()
    {

        if (_instance != null)
        {
           Destroy(gameObject);
        }
        else
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = 120;
        enemySort = Enum.GetValues(typeof(EnemyType)).Length;

        playerVM = new(statsSO);

       
        
    }

    void Start()
    {
        AudioManager.Instance.PlayBgm(true);
    }



    void Update()
    {

        if(!isLive) return;
        
        time += Time.deltaTime;

        if(EnemyCnt <= 0 && spawnTime < time)
        {
            
            for(int i=0; i<enemySort; ++i)
                StartCoroutine(Spawn(1,i));
            statsSO.CurHP.Value = statsSO.GetStat(StatType.MaxHP).value.Value;
               
            EnemyCnt = spawnCnt;

        }

        IsMove = !IsEnemy();
    }

    bool IsEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);

            if (viewPos.x >= 0 && viewPos.x <= 1 &&
                viewPos.y >= 0 && viewPos.y <= 1 &&
                viewPos.z > 0)
            {
                return true; // 하나라도 화면 안에 있으면 true
            }
        }

        return false; // 모두 화면 밖에 있음
    }


    WaitForSeconds wait = new WaitForSeconds(1f);

    IEnumerator Spawn(int cnt,int type)
    {

        yield return wait;

        EnemyFactory.Spawn(cnt,type);


    }

    



    



}
