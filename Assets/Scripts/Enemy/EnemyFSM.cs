using System;
using System.Collections;
using BigInteger = System.Numerics.BigInteger;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Assets.Scripts;



public class EnemyFSM : MonoBehaviour,IPoolable
{

    public event Action<GameObject,int> ReturnEvent;
    public event Action<int,Vector3> DropItemEvent;
   
    public event Action<Vector3,FinalDamage> DamageEvent;


    bool isLive;
   
    
    public Transform player;
    State state;

    BigInteger curHP;

    [SerializeField]
    StatsSO pStats;

    float attackTime = 0;

    FinalDamage fDamage = new(0,false);

    public EnemySO enemySO;
    Animator anim; 
    WaitForFixedUpdate wait = new ();

    Rigidbody2D rigid;
    public float hpRate = 1.01f;
    public float atkRate = 1.01f;

    public int exp = 1;

    int pLevel = 0;
    BigInteger maxHP;
    BigInteger MaxHP => Utility.GeoProgression(enemySO.maxHP,hpRate,pStats.Level.Value);
    
    BigInteger attackPower;
    
    BigInteger AttackPower => Utility.GeoProgression(enemySO.attackPower,atkRate,pStats.Level.Value);

   
    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemySO.animCon;
        rigid = GetComponent<Rigidbody2D>();

        attackPower = AttackPower;
       
        
    }
    void OnEnable()
    {
        isLive = true;
        state = State.Run;
        SetEnemy();
        
        print(curHP);
    }

    void SetEnemy()
    {
        if(pLevel == pStats.Level.Value)
        {
            curHP = maxHP;

            
        }
        else
        {
            pLevel = pStats.Level.Value;
            maxHP = MaxHP;
            curHP = maxHP;

            attackPower = AttackPower;
          
        }

    }

    void Update()
    {
        
        if(GameManager.Instance.Stop)
            return;

       
        if(!GameManager.Instance.isLive)
            ReturnEvent?.Invoke(gameObject,(int)enemySO.type);

        
        switch(state)
        {
            case State.Idle:
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                if(isLive)
                    Die();
                break;

        }
        
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.Stop)
            return;
        switch(state)
        {
            
            case State.Run:
                Run();
                break;

        }
        
    }

   

    void Run()
    {
       

        float distance = Vector3.Distance(transform.position, player.position);

        float stopDistance = 3f;

        float moveSpeed = enemySO.moveSpeed;


        if (distance <= stopDistance)
        {
            state = State.Attack;
            return;
        }

        Vector3 dir = (player.position - transform.position).normalized;

        // y축 이동 제거 → x축 방향만 사용
        dir = new Vector3(dir.x, 0, 0);

        
        // ✅ Rigidbody2D 기반 이동
        Vector2 nextPos = rigid.position + new Vector2(dir.x, 0) * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(nextPos);


        // 캐릭터 방향 반전
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? 1 : -1;
            transform.localScale = scale;
        }

       
        
        
    }


    void Attack()
    {
       
        float distance = Vector3.Distance(transform.position, player.position);

        float runDistance = 3f;

        if (distance >= runDistance)
        {
            attackTime = 0;
            state = State.Run;
            return;
        }

        attackTime += Time.deltaTime;
       
        if(attackTime >= enemySO.attackDelay)
        {
            attackTime = 0;
            pStats.CurHP.Value -= attackPower;
            
        }
    
    }

    public void HitEnemy(FinalDamage fDamage,float delay)
    {
        this.fDamage = fDamage;
       
        StartCoroutine(HitDelay(delay));

       
        
    }

    void Die()
    {
        isLive = false;
        
        anim.SetTrigger("Die");
        
        DropItemEvent?.Invoke((int)ItemType.Gold,transform.position);

        ReturnEvent?.Invoke(gameObject,(int)enemySO.type);

        pStats.AddExp(exp);

      
      

    }

    IEnumerator HitDelay(float delay)
    {
        // 스파인 애니메이션에 맞춰 딜레이 ( 스파인 에디터 사용 불가 이슈)
        yield return new WaitForSeconds(delay);
        
       
        curHP -= fDamage.damage;
        if(curHP > 0)
        {
           
            StartCoroutine(KnockBack());
           
        }  
        else
        {

            state = State.Die;
        }
        DamageEvent?.Invoke(transform.position,fDamage);
           
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 dir = (transform.position - player.position).normalized;
        Vector2 velocity = rigid.linearVelocity;
        rigid.linearVelocity = Vector2.zero;
        rigid.AddForce(dir * 2f, ForceMode2D.Impulse);

        state = State.Idle;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(1f); // 스턴 기간 
        anim.SetTrigger("Exit");
        state = State.Run;
        rigid.linearVelocity = velocity;



    }
   


}




