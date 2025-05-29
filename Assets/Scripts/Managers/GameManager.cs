using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{


   
    float time;
    float spawnTime = 2f;
    

    #region Ads


    #endregion

    int enemySort;
    int _enemyCnt;
    public int EnemyCnt
    {
        get => _enemyCnt;
        set { _enemyCnt = value; }
    }

    int spawnCnt = 5;
    public EnemyPool enemyPool;

    public StatsSO statsSO;

    [HideInInspector]
    public StatsVM statsVM;

    #region
    private bool _isLive;
    public bool IsLive
    {
        get => _isLive;
        set
        {
            _isLive = value;
        }
    }
    private bool _isLoding;
    public bool IsLoding
    {
        get => _isLoding;
        set
        {
            _isLoding = value;
        }
    }

    bool _pause = false;
    public bool Pause
    {
        get => _pause;
        set
        {
            _pause = value;
        }
    }
    #endregion



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
        Pause = true;
        Application.targetFrameRate = 120; 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        enemySort = Enum.GetValues(typeof(EnemyType)).Length;

        statsVM = new(statsSO);
        
    }

    void Start()
    {
        
        AudioManager.Instance.PlayBGM(true);
        
       
    }



    void Update()
    {
    
        if(Pause) return;
        if(!IsLive) return;
        if(IsLoding) return;
        
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

        enemyPool.Spawn(cnt,type);


    }

    



    



}
