public abstract class EnemyStateBase // значит что от этого класса наследуются состояния
{
    protected EnemyController enemy; // Ссылка на главный скрипт врага
    protected StateMachine machine; // На скрипт автомата

    public EnemyStateBase(EnemyController enemy, StateMachine machine) // Срабатывает в момент создания состояния и передает ссылки
    {
        this.enemy = enemy;
        this.machine = machine;
    }

    // Virtual значит чтонаследники могут их переписать под себя
    public virtual void Enter() { } 
    public virtual void Tick() { } // Update
    public virtual void PhysicsTick() { } // FixedUpdate
    public virtual void Exit() { }
}