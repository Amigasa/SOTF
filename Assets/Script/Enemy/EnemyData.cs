using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Patrol")]
    public float patrolSpeed = 2f;

    [Header("Chase")]
    public float chaseSpeed = 3.5f;
    public float detectRadius = 5f;   // радиус, в котором враг замечает игрока
    public float loseRadius = 7f;     // если игрок дальше — теряем

    [Header("Attack")]
    public float attackRadius = 1.2f;
    public float attackCooldown = 1f;
    public int attackDamage = 1;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask groundLayer;
}
