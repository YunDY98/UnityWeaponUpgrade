using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    Transform player;
    EState eState;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    void OnEnable()
    {
        eState = EState.Run;
    }

    void Update()
    {
        switch(eState)
        {
            case EState.Idle:
                break;
            case EState.Run:
                Run();
                break;
        }
        
    }

    void Run()
    {
        
        Vector3 dir = (player.position - transform.position).normalized;

        // y축 이동 제거 → x축 방향만 사용
        dir = new Vector3(dir.x, 0, 0);

        float moveSpeed = 1f;

        transform.position += dir * moveSpeed * Time.deltaTime;

        // 캐릭터 방향 반전 (왼쪽/오른쪽 보기)
        if (dir.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? 1 : -1; // 오른쪽이면 1, 왼쪽이면 -1
            transform.localScale = scale;
        }
    
        
        
    }
}



enum EState
{
    Idle,
    Run,
    Attack,
    Die
}