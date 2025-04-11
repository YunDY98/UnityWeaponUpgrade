using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFactory: ObjectPool<EnemyFSM>
{

    WaitForSeconds spawnDelay = new WaitForSeconds(1f);

    public void Spawn(int cnt)
    {  
       StartCoroutine(SpawnDelay(cnt));
       
    }

    IEnumerator SpawnDelay(int cnt)
    {
        for(int i=0; i <cnt; ++i)
        {
            if(pool.Count > 0)
            {
                var enemy =  pool.Dequeue();
                enemy.gameObject.SetActive(true);
                enemy.transform.position = spawnPos.position;
                yield return spawnDelay;
            }
            else
            {
                Create();
                i--;
               
            }

        }
      
       
    }
}
