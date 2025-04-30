using UnityEngine;

using Spine.Unity;

using System.Collections.Generic;
using System.Collections;
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
    
    int stayEnemy = 0;


    bool isAtk = false;
    
    Coroutine atkCor;


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
        
    }

    public void Init()
    {
        GameManager.Instance.isLive = true;
        GameManager.Instance.IsMove = true;
        state = State.Run;

        
    }

    

    void Update()
    {
        print(state);
       
        if(GameManager.Instance.Stop)
            return;
       
        if(!GameManager.Instance.isLive) return;
        

        if(statsSO.CurHP.Value <= 0)
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
                if(!isAtk)
                {
                    Attack();
                }
                    
                break;

        }
      

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
       
        if(other.CompareTag("Enemy"))
        {
            stayEnemy++;
            state = State.Attack;
        }
        
    }



    void OnTriggerExit2D(Collider2D collision)
    {
       
        if(collision.CompareTag("Enemy"))
        {
            stayEnemy--;
        }
        
           
        if(stayEnemy <= 0)
        {
            stayEnemy = 0;
            state = State.Idle;
        }
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
       
        if(attackDelay > statsSO.GetStat(StatType.AttackSpeed).GetFValue())
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
        
        //공격 코루틴후 죽을시 
        if(atkCor != null)
        {
            StopCoroutine(atkCor);
            isAtk = false;
        }
    }



    public void Atk()
    {
        
        float delay = 0.6f; // 스파인 애니메이션에 맞춰서 딜레이
        cnt = (int)statsSO.GetStat(StatType.AttackCnt).value.Value;
        center = transform.position;
        // 박스 중심과 크기 설정
        Vector2 size = new Vector2((float)statsSO.GetStat(StatType.AttackRange).GetFValue(), 3f);
        float angle = 0f;

        // 적 레이어 마스크
        int enemyLayer = LayerMask.GetMask("Enemy");
       
        // 공격 범위 내의 모든 적 감지
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, angle, enemyLayer);

      
        // 적과의 거리를 계산하고 정렬하기 위한 리스트 생성
        List<(Collider2D collider, float distance)> sortedEnemies = new List<(Collider2D, float)>();
        
        foreach (Collider2D col in colliders)
        {
            float distance = Vector2.Distance(transform.position, col.transform.position);
            sortedEnemies.Add((col, distance));
        }
        
        // 거리를 기준으로 정렬
        sortedEnemies.Sort((a, b) => a.distance.CompareTo(b.distance));
        
        // 정렬된 순서대로 공격
        foreach (var enemyData in sortedEnemies)
        {  
            if(0 < cnt)
            {
                EnemyFSM enemy = enemyData.collider.GetComponent<EnemyFSM>();
                if (enemy != null)
                {
                    
                    atkCor = StartCoroutine(HitDelay(Sfx.Attack, enemy, delay));
                    cnt--;
                }
            }
            else
            {
               
                break;
            }
        }
    }

    IEnumerator HitDelay(Sfx sfx,EnemyFSM enemy,float delay)
    {
        isAtk = true;
        yield return new WaitForSeconds(delay);
        enemy.HitEnemy(statsSO.Damage());
        AudioManager.Instance.PlaySfx(sfx);
        isAtk = false;
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



   

