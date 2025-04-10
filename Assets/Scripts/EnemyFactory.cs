using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFactory: MonoBehaviour
{
    public GameObject[] objects;

    public Transform spawnPos;

    public Queue<GameObject> pool = new();

    WaitForSeconds spawnDelay = new WaitForSeconds(1f);


    void Create()
    {
        foreach(var obj in objects)
        {
            var enemy = Instantiate(obj,transform);
            enemy.GetComponent<EnemyFSM>().ReturnEvent += Return;
            enemy.SetActive(false);
            pool.Enqueue(enemy);

        }

    }

    public void Return(GameObject enemy)
    {
        pool.Enqueue(enemy);
        enemy.gameObject.SetActive(false);

    }

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
