using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    public GameObject target;

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if(other.gameObject == target)
        {
            Debug.Log("Attack");
        }
        
    }

    

}
