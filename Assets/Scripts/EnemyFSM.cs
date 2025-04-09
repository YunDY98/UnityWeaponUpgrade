using System.Collections;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    Transform player;
    State state;

    int curHP;

    float currentTime = 0;

    [SerializeField]
    EnemySO enemySO;
    Animator anim;

    WaitForSeconds delay = new(1f);

    void Awake()
    {
        anim = GetComponent<Animator>();
        curHP = enemySO.maxHP;
        
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
            case State.Run:
                Run();
                break;
            case State.Attack:
                Attack();
                break;

        }
        
    }

    void Run()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

      
        float stopDistance = 1.5f;

        if (distance <= stopDistance)
        {
            state = State.Attack;
            return;
        }
        
        Vector3 dir = (player.position - transform.position).normalized;

        // y축 이동 제거 → x축 방향만 사용
        dir = new Vector3(dir.x, 0, 0);

        float moveSpeed = enemySO.moveSpeed;

        transform.position += dir * moveSpeed * Time.deltaTime;

        // 캐릭터 방향 반전 (왼쪽/오른쪽 보기)
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? 1 : -1; // 오른쪽이면 1, 왼쪽이면 -1
            transform.localScale = scale;
        }
    
        
        
    }

    void Attack()
    {

       
        
        currentTime += Time.deltaTime;
       
        if(currentTime >= enemySO.attackDelay)
        {
            currentTime = 0;
            PlayerStats.Instance.HP -= enemySO.attackPower;
            
        }
       


    }

  
}




