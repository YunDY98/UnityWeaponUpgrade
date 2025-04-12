
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    [Header("기본 능력치")]
    public string enemyName;

    public EnemyType type;
    public RuntimeAnimatorController animCon;
    public int maxHP;
    public int attackPower;
    public float moveSpeed;
    public float attackDelay;
}

public enum EnemyType
{
    Enemy0,
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
}