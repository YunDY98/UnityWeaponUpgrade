using System.Collections;
using System.Numerics;
using UnityEngine;

public class EnemyFactory: ObjectPool
{
    [SerializeField]
    Transform player;

    [SerializeField]
    StatsSO StatsSO;

    [SerializeField]
    ItemPool itemPool;

    [SerializeField]
    EffectManager effect;


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
           // enemy.statsSO = StatsSO;
            enemy.player = player;
            enemy.DamageEvent += effect.Damage;
            enemy.ReturnEvent += Return;
            enemy.DropItemEvent += itemPool.DropItem;
           
           
            
            
            tmp.SetActive(false);
            
            
           
            pool[(int)enemy.enemySO.type].Enqueue(tmp);

        }

    }
    public override void Return(GameObject obj,int type)
    {
        pool[type].Enqueue(obj);
        obj.SetActive(false);
        GameManager.Instance.EnemyCnt -= 1;


    }

    
    



}
