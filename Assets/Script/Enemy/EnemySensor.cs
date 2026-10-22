using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [Header("Data")]
    public EnemyData data; // Сюда вставляем тот объект с настройками

    [Header("Wall Check")]
    public Transform wallCheck; // Точка впереди врага
    public float wallCheckRadius = 0.1f; // Радиус этой точки

    public Transform Player { get; private set; }
    public float DistanceToPlayer { get; private set; } = float.MaxValue; // Дистанция до игрока

    private void Update() // Каждый кадр
    {
        if (data == null) return; // Чтобы не было ошибки если забыли перетащить ассет данных в инспекторе

        // Ищем игрока
        Collider2D hit = Physics2D.OverlapCircle(transform.position, data.loseRadius, data.playerLayer); // Рисует круг и ищет коллайдеры на слое игрока

        if (hit != null) // Если нашли что-то в радиусе
        {
            Player = hit.transform; // Ссылка на игрока
            DistanceToPlayer = Vector2.Distance(transform.position, Player.position); // Подсчет точного расстояния до игрока через тиорему Пифагора, для понимания бить или не бить игрока
        }
        else // Иначе если не найден
        {
            Player = null; // Обнуляем цель
            DistanceToPlayer = float.MaxValue; // Делаем расстояние макс возможным чтобы проверки возвращали false
        }
    }

    // Вопросы к сенсору возвращающие правду или ложь
    public bool PlayerInDetectRange() => Player != null && DistanceToPlayer <= data.detectRadius;
    public bool PlayerInAttackRange() => Player != null && DistanceToPlayer <= data.attackRadius;
    public bool PlayerLost() => Player == null || DistanceToPlayer > data.loseRadius;

    // Проверка стены впереди
    public bool WallAhead()
    {
        if (wallCheck == null) return false;
        return Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, data.groundLayer); // Проверяет в радиусе есть ли объект на слое стены
    }

    // Рисуем круги в редакторе для отладки чисто для удобства
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