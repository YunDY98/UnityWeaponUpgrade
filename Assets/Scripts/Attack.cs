using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    protected List<GameObject> enemies = new(); // 적 배열

    int cnt;
    float range;

    

    void MultiAttack()
    {
        // 주변의 적을 감지
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        enemies.Clear(); // 리스트 초기화
        foreach(Collider other in colliders)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemies.Add(other.gameObject); // 적을 리스트에 추가
            }
        }

        // cnt 수만큼 적 공격 
    
        foreach(GameObject enemy in enemies)
        {
            if(0 < cnt)
            {
                // EnemyFSM 컴포넌트 가져오기
                EnemyFSM efsm = enemy.GetComponent<EnemyFSM>();
                if (efsm != null)
                {
                    
                    cnt--;
                }
            }
            else
            {
                break; // 최대 공격수에 도달하면 루프 종료
            }
        }
    }


}
