using UnityEngine;

using Spine.Unity;

using System.Collections.Generic;
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

    #region Attack
    List<GameObject> enemies = new(); // 적 배열
    int cnt; // 공격 가능 적 수 
    float attackDelay = 0f; // 공격 시간 
    Vector2 center;
    #endregion

    [SerializeField]
    StatsSO statsSO;
    
    
    


    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    // Start is called before the first frame update


    void Start()
    {
        

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
       
        Init();

        statsSO.HP.Value = statsSO.MaxHP.Value;
        
    }

    void Init()
    {
        GameManager.Instance.isLive = true;
        GameManager.Instance.IsMove = true;
        state = State.Run;

        

        
        
    }

    void Update()
    {
       
        if(!GameManager.Instance.isLive) return;

        if(statsSO.HP.Value <= 0)
            state = State.Die;
            
            
        
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
        if(attackDelay > statsSO.AttackDelay.Value)
        {
            attackDelay = 0f;
            spineAnimationState.SetAnimation(0, atkAnimationName_1, false);

        
            Atk();
           

        }
        
    }

    public void Die()
    {
        GameManager.Instance.isLive = false;
        GameManager.Instance.IsMove = false;
        SetAnim(deathAnimationName,false);
       
    }



    public void Atk()
    {
        float delay = 0.6f; // 스파인 애니메이션에 맞춰서 딜레이
        cnt = statsSO.AttackCnt.Value;
        center = transform.position;
        // 박스 중심과 크기 설정
      
        Vector2 size = new Vector2(statsSO.AttackRange.Value, 3f);
        float angle = 0f;

        // 적 레이어 마스크
        int enemyLayer = LayerMask.GetMask("Enemy");
       
        // 공격 범위 내의 모든 적 감지
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, angle, enemyLayer);
      
        // 감지된 적마다 데미지 처리
        foreach (Collider2D col in colliders)
        { 
            if(0 < cnt)
            {
               
                EnemyFSM enemy = col.GetComponent<EnemyFSM>();
                if (enemy != null)
                {
                    
                    enemy.HitEnemy(statsSO.Damage(),delay);
                    cnt--;
                }

            }
            else
            {
                break;
            }
            
        }

        
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



   

