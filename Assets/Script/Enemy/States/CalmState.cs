public class CalmState : EnemyCompositeState
{
    public PatrolState Patrol { get; private set; }

    public CalmState(EnemyController e, StateMachine m) : base(e, m)
    {
        Patrol = new PatrolState(e, m);
    }

    public override void Enter()
    {
        enemy.IsDangerous = false; // В спокойствии враг безопасен
        ChangeChild(Patrol);       // По умолчанию патрулируем
    }
}