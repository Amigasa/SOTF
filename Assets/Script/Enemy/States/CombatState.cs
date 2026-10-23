using UnityEngine;

public class CombatState : EnemyCompositeState
{
    // Ссылки на состояния боя (Погоня и Атака)
    public ChaseState Chase { get; private set; }
    public AttackState Attack { get; private set; }

    public CombatState(EnemyController e, StateMachine m) : base(e, m)
    {
        Chase = new ChaseState(e, m);
        Attack = new AttackState(e, m);
    }

    public override void Enter()
    {
        enemy.IsDangerous = true; // В бою враг опасен
        ChangeChild(Chase);       // Всегда начинаем бой с погони
    }

    public override void Tick()
    {
        // ЛОГИКА ПЕРЕКЛЮЧЕНИЯ ВНУТРИ БОЯ: Погоня <-> Атака
        if (enemy.Sensor.PlayerInAttackRange() && CurrentChild != Attack)
        {
            ChangeChild(Attack);
        }
        else if (!enemy.Sensor.PlayerInAttackRange() && CurrentChild != Chase)
        {
            ChangeChild(Chase);
        }

        base.Tick();
    }
}