using UnityEngine;

// Это класс для родительских состояний (Alive, Combat)
public class EnemyCompositeState : EnemyStateBase
{
    // Ссылка на текущее дочернее состояние
    public EnemyStateBase CurrentChild { get; private set; }

    public EnemyCompositeState(EnemyController e, StateMachine m) : base(e, m) { }

    // Метод для переключения дочерних состояний внутри родителя
    public void ChangeChild(EnemyStateBase next)
    {
        if (next == null || next == CurrentChild) return;

        CurrentChild?.Exit(); // Выходим из старого
        CurrentChild = next;  // Запоминаем новое
        CurrentChild.Enter(); // Входим в новое
    }

    public override void Tick()
    {
        // Сначала работает логика родителя (если есть), затем ребенка
        CurrentChild?.Tick();
    }

    public override void PhysicsTick()
    {
        CurrentChild?.PhysicsTick();
    }
}