using UnityEngine;

public class DeadState : EnemyStateBase
{
    public DeadState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.IsDangerous = false; // Мертвый враг безопасен
        enemy.Body.velocity = Vector2.zero;

        var col = enemy.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Object.Destroy(enemy.gameObject, 0.5f);
    }
}