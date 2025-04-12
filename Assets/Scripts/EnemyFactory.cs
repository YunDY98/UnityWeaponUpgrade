using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyFactory: ObjectPool<EnemyFSM>
{
    [SerializeField]
    Transform player;

    [SerializeField]
    StatsSO StatsSO;

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

    protected override void Create()
    {
        foreach(var obj in objects)
        {
            var tmp = Instantiate(obj,transform);

            var enemy = tmp.GetComponent<EnemyFSM>();
            
            enemy.ReturnEvent += Return;
            enemy.statsSO = StatsSO;
            enemy.player = player;
            tmp.SetActive(false);
            pool.Enqueue(tmp);

        }

    }
}
