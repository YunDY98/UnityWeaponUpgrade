using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float time;
    float spawnTime = 2f;
    int _enemyCnt;
    public int EnemyCnt
    {
        get { return _enemyCnt; }
        set { _enemyCnt = value; }
    }

    int spawnCnt = 5;
    public EnemyFactory EnemyFactory;


    private static GameManager _instance;
    public static GameManager Instance
    {
        get{ return _instance;}


    }
    private bool _isMove;

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

        Application.targetFrameRate = 60;

       
        
    }

    void Start()
    {
        IsMove = true;

        
       
         
    }

    void Update()
    {
        
        time += Time.deltaTime;

        if(EnemyCnt <= 0 && spawnTime < time)
        {
            StartCoroutine(Spawn(spawnCnt));
            EnemyCnt = spawnCnt;

        }

        if(IsEnemy())
        {
            IsMove = false;
            
        }
        else
        {
            IsMove = true;
        }
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


    WaitForSeconds wait = new WaitForSeconds(2f);

    IEnumerator Spawn(int cnt)
    {

        yield return wait;

        EnemyFactory.Spawn(cnt);


    }




    



}
