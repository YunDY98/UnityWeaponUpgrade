using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string walkAnimationName;

    [SpineAnimation]
    public string atkAnimationName_1;

    [SpineAnimation]
    public string hitAnimationName;

    [SpineAnimation]
    public string deathAnimationName;

    [SpineAnimation]
    public string stunAnimationName;

    [SpineAnimation]
    public string skillAnimationName_1;
    [SpineAnimation]
    public string skillAnimationName_2;
    [SpineAnimation]
    public string skillAnimationName_3;

    #endregion
   
    [HideInInspector]
    public State state;
    SkeletonAnimation skeletonAnimation;

    Attack attack;
    
    
    float attackDelay = 0f; // 공격 시간 

    
    bool isLive;

    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    // Start is called before the first frame update

    void Awake()
    {
        attack = GetComponentInChildren<Attack>();
       
        
    }
    void Start()
    {
        

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
       
        Init();
    }

    void Init()
    {
        isLive = true;
        state = State.Run;
        
    }

    void Update()
    {
        if(!isLive) return;
            
        
        if(GameManager.Instance.IsMove)
        {
            if(state != State.Die)
                state = State.Run;
        }
        
        switch(state)
        {
            case State.Idle: 
                Idle();
                break;
            case State.Run:
                Run();
                break;
            case State.Die:
                Die();
                break;
            case State.Attack:
                Attack();
                break;

        }
      

    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.CompareTag("Enemy"))
        {
            state = State.Attack;
        }
        
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        
        state = State.Idle;
    }

    public void SetAnim(string animName,bool loop = true)
    {
        string current = skeletonAnimation.AnimationName;

        if(current != animName)
            spineAnimationState.SetAnimation(0, animName, loop);

    }

    public void Run()
    {
        SetAnim(runAnimationName);
        if(!GameManager.Instance.IsMove)
        {
            state = State.Idle;
        }
    }
    public void Idle()
    {
        SetAnim(idleAnimationName);
    }

    public void Attack()
    {
        
        attackDelay += Time.deltaTime;
        if(attackDelay > PlayerStats.Instance.AttackDelay)
        {
            attackDelay = 0f;
            spineAnimationState.SetAnimation(0, atkAnimationName_1, false);

        
            attack.Atk();
           

        }
        
    }

    public void Die()
    {
        isLive = false;
        SetAnim(deathAnimationName,false);
       
    }




    
}


public enum State
{
    Idle,
    Run,
    Die,
    Damaged,
    Attack,
    Buff = 20,

   
}



   

