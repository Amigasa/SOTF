using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")] // Для того чтобы создать файл с настройками
public class EnemyData : ScriptableObject
{
    [Header("Patrol")] // Удобные заголовки
    public float patrolSpeed = 2f; // Скорость ходьбы

    [Header("Chase")]
    public float chaseSpeed = 3.5f; // Скорость бега
    public float detectRadius = 5f; // радиус, в котором враг замечает игрока
    public float loseRadius = 7f; // если игрок дальше — теряем

    [Header("Attack")]
    public float attackRadius = 1.2f; // Радиус удара
    public float attackCooldown = 1f; // Задержка между ударами
    public int attackDamage = 1; // Урон

    [Header("Layers")]
    public LayerMask playerLayer; // Слой игрока
    public LayerMask groundLayer; // Слой стен
}
