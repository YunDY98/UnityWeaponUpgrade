using System.Collections;

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
           // enemy.statsSO = StatsSO;
            enemy.player = player;
            enemy.DamageEvent += EnemyUIManager.Instance.Damage;
            enemy.ReturnEvent += Return;
            enemy.DropItemEvent += itemPool.DropItem;
            enemy.AttackEvent += Attack;
            enemy.AddExpEvent += AddExp;
            
            
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

    public void Attack(int atk)
    {

        StatsSO.CurHP.Value -= atk;


    }

    public void AddExp(int exp)
    {
        StatsSO.AddExp(exp);
    }
     
    



}
