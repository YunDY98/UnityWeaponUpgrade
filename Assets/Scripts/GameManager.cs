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

    PState _State;

    public PState State
    {
        get{ return _State;}
        set{ _State = value; }
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
        State = PState.Move;
         
    }





    



}
public enum PState
{
    Idle,
    Move,
    Die,
    Damaged,
    Attack,
    Buff = 20,

   
}