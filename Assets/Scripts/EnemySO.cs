
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    [Header("기본 능력치")]
    public string enemyName;
    public int maxHP;
    public int attackPower;
    public float moveSpeed;
    public float attackDelay;
}
