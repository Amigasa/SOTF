public class StateMachine
{
    public EnemyStateBase Current { get; private set; }

    public void ChangeState(EnemyStateBase next)
    {
        if (next == null || next == Current) return;
        Current?.Exit();
        Current = next;
        Current.Enter();
    }

    public void Tick() => Current?.Tick();
    public void PhysicsTick() => Current?.PhysicsTick();
}