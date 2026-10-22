public abstract class EnemyStateBase
{
    protected EnemyController enemy;
    protected StateMachine machine;

    public EnemyStateBase(EnemyController enemy, StateMachine machine)
    {
        this.enemy = enemy;
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }         // Update
    public virtual void PhysicsTick() { }  // FixedUpdate
    public virtual void Exit() { }
}