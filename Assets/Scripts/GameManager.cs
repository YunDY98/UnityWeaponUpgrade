using UnityEngine;

public class GameManager : MonoBehaviour
{
   


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

        foreach (GameObject enemy in enemies)
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





    



}
