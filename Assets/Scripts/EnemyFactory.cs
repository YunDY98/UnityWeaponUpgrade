using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyFactory: ObjectPool
{
    [SerializeField]
    Transform player;

    [SerializeField]
    StatsSO StatsSO;

    [SerializeField]
    ItemPool itemPool;



    WaitForSeconds spawnDelay = new (1f);


    public void Spawn(int cnt,int type)
    {  
       StartCoroutine(SpawnDelay(cnt,type));
       
    }

    IEnumerator SpawnDelay(int cnt,int type)
    {
        for(int i=0; i <cnt; ++i)
        {
            if(pool[type].Count > 0)
            {
                var enemy =  pool[type].Dequeue();
                enemy.SetActive(true);
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

    protected override void Create(int type = 0)
    {
        foreach(var obj in objects)
        {
            var tmp = Instantiate(obj,transform);

            var enemy = tmp.GetComponent<EnemyFSM>();
            
            enemy.ReturnEvent += Return;
            enemy.DropItemEvent += itemPool.DropItem;
            enemy.statsSO = StatsSO;
            enemy.player = player;
            tmp.SetActive(false);
            pool[(int)enemy.enemySO.type].Enqueue(tmp);

        }

    }

}
