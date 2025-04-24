using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if(other.CompareTag("Player"))
        {
            Debug.Log("Attack");
        }
        
    }

    

}
