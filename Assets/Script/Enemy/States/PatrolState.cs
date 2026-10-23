using UnityEngine;

public class PatrolState : EnemyStateBase
{
    private int direction = -1; // -1 влево, +1 вправо

    public PatrolState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.IsDangerous = false; // В патруле враг безопасен

        // Если при возврате в патруль враг стоит лицом к стене — сразу разворачиваем его
        if (enemy.Sensor.WallAhead())
        {
            direction *= -1;
        }
    }                                                                                                                                                       

    public override void PhysicsTick()
    {
        // Если впереди стена, меняем направление на противоположное
        if (enemy.Sensor.WallAhead())
        {
            direction *= -1;
        }

        // Двигаемся в текущем направлении
        enemy.Body.velocity = new Vector2(direction * enemy.Data.patrolSpeed, enemy.Body.velocity.y);
        enemy.FaceDirection(direction);
        enemy.Anim.SetFloat("Speed", Mathf.Abs(enemy.Body.velocity.x));
    }
}