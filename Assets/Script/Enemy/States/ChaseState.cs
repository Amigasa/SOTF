using UnityEngine;

public class ChaseState : EnemyStateBase
{
    public ChaseState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.IsDangerous = true; // В погоне враг опасен
    }

    public override void Tick()
    {
        if (enemy.Sensor.PlayerLost())
        {
            machine.ChangeState(enemy.PatrolState);
            return;
        }

        if (enemy.Sensor.PlayerInAttackRange())
            machine.ChangeState(enemy.AttackState);
    }

    public override void PhysicsTick()
    {
        if (enemy.Sensor.Player == null) return;

        float dir = Mathf.Sign(enemy.Sensor.Player.position.x - enemy.transform.position.x);
        enemy.Body.velocity = new Vector2(dir * enemy.Data.chaseSpeed, enemy.Body.velocity.y);
        enemy.FaceDirection((int)dir);
    }
}