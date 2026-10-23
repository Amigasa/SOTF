public class AliveState : EnemyCompositeState
{
    // Дочерние состояния для "живого" режима
    public CalmState Calm { get; private set; }
    public CombatState Combat { get; private set; }

    public AliveState(EnemyController e, StateMachine m) : base(e, m)
    {
        Calm = new CalmState(e, m);
        Combat = new CombatState(e, m);
    }

    public override void Enter()
    {
        // При оживлении (или старте игры) начинаем с мирного режима
        ChangeChild(Calm);
    }

    public override void Tick()
    {
        // ВЫСОКОУРОВНЕВАЯ ЛОГИКА: Проверяем условия для смены "ветки"
        if (enemy.Sensor.PlayerInDetectRange() && CurrentChild != Combat)
        {
            ChangeChild(Combat); // Если увидели игрока — переключаемся на Бой
        }
        else if (enemy.Sensor.PlayerLost() && CurrentChild != Calm)
        {
            ChangeChild(Calm); // Если потеряли — переключаемся на Спокойствие
        }

        // Вызываем логику текущего ребенка
        base.Tick();
    }
}