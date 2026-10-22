using UnityEngine;

public class AttackState : EnemyStateBase
{
    private float lastAttackTime = -999f;

    public AttackState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.Body.velocity = new Vector2(0, enemy.Body.velocity.y);
        enemy.IsDangerous = true; // В атаке враг опасен
    }

    public override void Tick()
    {
        if (Time.time - lastAttackTime >= enemy.Data.attackCooldown)
        {
            lastAttackTime = Time.time;
            enemy.PerformAttack();
        }

        if (!enemy.Sensor.PlayerInAttackRange())
            machine.ChangeState(enemy.ChaseState);

        if (enemy.Sensor.PlayerLost())
            machine.ChangeState(enemy.PatrolState);
    }

    public override void PhysicsTick()
    {
        enemy.Body.velocity = new Vector2(0, enemy.Body.velocity.y);
    }
}