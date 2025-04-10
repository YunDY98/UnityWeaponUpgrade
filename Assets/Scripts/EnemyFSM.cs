using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyFSM : MonoBehaviour
{

    public event Action<GameObject> ReturnEvent;
    
    Player player;
    State state;
    
    [SerializeField]
    float HP;

    float attackTime = 0;

    [SerializeField]
    EnemySO enemySO;
    Animator anim; 
    WaitForFixedUpdate wait = new ();

    Rigidbody2D rigid;
    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = enemySO.animCon;
        rigid = GetComponent<Rigidbody2D>();
        HP = enemySO.maxHP;
      

        
    }

    void Start()
    {
        player = PlayerStats.Instance.player;
        
        
    }

    void OnEnable()
    {

        state = State.Run;
        

    }

    void Update()
    {
        switch(state)
        {
            case State.Idle:
                break;
            case State.Attack:
                Attack();
                break;
            case State.Die:
                Die();
                break;

        }
        
    }

    void FixedUpdate()
    {
        switch(state)
        {
            
            case State.Run:
                Run();
                break;

        }
        
    }

    void Run()
    {
       

        float distance = Vector3.Distance(transform.position, player.transform.position);

        float stopDistance = 2.2f;

        float moveSpeed = enemySO.moveSpeed;


        if (distance <= stopDistance)
        {
            state = State.Attack;
            return;
        }

        Vector3 dir = (player.transform.position - transform.position).normalized;

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
       
        float distance = Vector3.Distance(transform.position, player.transform.position);

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
            PlayerStats.Instance.HP -= enemySO.attackPower;
            
        }
    
    }

    public void HitEnemy(float damage,float delay)
    {
        
        StartCoroutine(HitDelay(damage,delay));
       
        
    }

    void Die()
    {

        GameManager.Instance.EnemyCnt -= 1;
        
        anim.SetTrigger("Dead");
        

        ReturnEvent?.Invoke(this.gameObject);

    }

 

    IEnumerator HitDelay(float damage,float delay)
    {
        // 스파인 애니메이션에 맞춰 딜레이 ( 스파인 에디터 사용 불가 이슈)
        yield return new WaitForSeconds(delay);
        
        HP -= damage;
        if(HP > 0)
        {
           
            StartCoroutine(KnockBack());
        }  
        else
        {
            state = State.Die;
        }
           
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 dir = (transform.position - player.transform.position).normalized;
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




