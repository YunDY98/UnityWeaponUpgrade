using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    List<GameObject> enemies = new(); // 적 배열
    int cnt; // 공격 가능 적 수 



    public void Atk()
    {
        float attackDelay = 0.6f; // 스파인 애니메이션에 맞춰서 딜레이
        cnt = PlayerStats.Instance.AttackCnt.Value;
        // 박스 중심과 크기 설정
        Vector2 center = transform.position;
        Vector2 size = new Vector2(PlayerStats.Instance.AttackRange.Value, 3f);
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
                    enemy.HitEnemy(PlayerStats.Instance.Damage.Value,attackDelay);
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
