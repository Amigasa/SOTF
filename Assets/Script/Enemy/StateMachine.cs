public class StateMachine
{
    public EnemyStateBase Current { get; private set; } // Current - свойство, которое хранит текущее состояние

    public void ChangeState(EnemyStateBase next) // Вызывается когда хотим переключить поведение
    {
        if (next == null || next == Current) return; //Если попытается переключить в то же самое состояние то ничего не произойдет
        Current?.Exit(); // Если курент пустой то старое состояние пока
        Current = next; // Меняем переменную
        Current.Enter(); // Новое состояние привет
    }

    public void Tick() => Current?.Tick(); // Передает команду обновления в текущее состояние
    public void PhysicsTick() => Current?.PhysicsTick();
}