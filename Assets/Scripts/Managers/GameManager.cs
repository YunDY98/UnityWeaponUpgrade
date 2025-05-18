using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public bool isLive;
    float time;
    float spawnTime = 2f;
    

    #region Ads

    public bool isReward = false;


    #endregion

    int enemySort;
    int _enemyCnt;
    public int EnemyCnt
    {
        get => _enemyCnt;
        set { _enemyCnt = value; }
    }

    int spawnCnt = 5;
    public EnemyFactory EnemyFactory;

    public StatsSO statsSO;

    [HideInInspector]
    public StatsVM statsVM;

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
        Stop = true;
        Application.targetFrameRate = 120; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        enemySort = Enum.GetValues(typeof(EnemyType)).Length;

        statsVM = new(statsSO);
        
        Loading.Instance.totalLoadCnt += statsSO.GetStats().Length;
        Loading.Instance.spriteLoadCnt += statsSO.GetStats().Length;
       
       
       
      
        
    }

    void Start()
    {
        
        AudioManager.Instance.PlayBGM(true);
        
       
    }



    void Update()
    {
    
        if(Stop) return;
        if(!isLive) return;
        
        time += Time.deltaTime;

        if(EnemyCnt <= 0 && spawnTime < time)
        {
            
            for(int i=0; i<enemySort; ++i)
                StartCoroutine(Spawn(1,i));
            statsSO.CurHP.Value = statsSO.GetStat((int)StatType.MaxHP).value.Value;
               
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
