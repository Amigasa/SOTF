using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data;

    [Header("Wall Check")]
    public Transform wallCheck;     // Точка впереди врага
    public float wallCheckRadius = 0.1f; // Радиус этой точки

    public Transform Player { get; private set; }
    public float DistanceToPlayer { get; private set; } = float.MaxValue;

    private void Update()
    {
        if (data == null) return;

        // Ищем игрока
        Collider2D hit = Physics2D.OverlapCircle(transform.position, data.loseRadius, data.playerLayer);

        if (hit != null)
        {
            Player = hit.transform;
            DistanceToPlayer = Vector2.Distance(transform.position, Player.position);
        }
        else
        {
            Player = null;
            DistanceToPlayer = float.MaxValue;
        }
    }

    public bool PlayerInDetectRange() => Player != null && DistanceToPlayer <= data.detectRadius;
    public bool PlayerInAttackRange() => Player != null && DistanceToPlayer <= data.attackRadius;
    public bool PlayerLost() => Player == null || DistanceToPlayer > data.loseRadius;

    // Проверка стены впереди
    public bool WallAhead()
    {
        if (wallCheck == null) return false;
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, data.groundLayer);
    }

    // Рисуем круги в редакторе для отладки
    private void OnDrawGizmosSelected()
    {
        if (data == null) return;

        // Зеленый - обнаружение
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.detectRadius);

        // Красный - атака
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRadius);

        // Оранжевый - проверка стены
        if (wallCheck != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
}