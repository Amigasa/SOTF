using UnityEngine;

public class DeadState : EnemyStateBase
{
    public DeadState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.IsDangerous = false; // Мертвый враг безопасен(
        enemy.Body.velocity = Vector2.zero;

        enemy.PlayDeathAnim(); // Вызываем анимацию

        var col = enemy.GetComponent<Collider2D>(); // Ищем коллайдер врага
        if (col) col.enabled = false; // И отключаем его чтобы не мешал игроку ходить

        Object.Destroy(enemy.gameObject, 1f); // Удаляем со сцены МУХАХА
    }
}