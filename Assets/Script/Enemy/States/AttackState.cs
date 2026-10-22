using UnityEngine;

public class AttackState : EnemyStateBase
{
    private float lastAttackTime = -999f; // Переменная для хранения времени последнего удара

    public AttackState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter() // Обнуляем скорость по X (чтобы враг остановился перед игроком) и делаем его опасным.
    {
        enemy.Body.velocity = new Vector2(0, enemy.Body.velocity.y);
        enemy.IsDangerous = true; // В атаке враг опасен
    }

    public override void Tick()
    {
        // Time.time это текущее время игры
        if (Time.time - lastAttackTime >= enemy.Data.attackCooldown) // Если с момента последнего удара прошло больше, чем attackCooldown
        {
            lastAttackTime = Time.time; // Мы бьем и запоминаем время удара
            enemy.PerformAttack(); 
        }

        if (!enemy.Sensor.PlayerInAttackRange()) // И проверяем не убежал ли игрок из радиуса атаки
            machine.ChangeState(enemy.ChaseState);

        if (enemy.Sensor.PlayerLost()) // И вообще из зоны видимости
            machine.ChangeState(enemy.PatrolState);
    }

    public override void PhysicsTick()
    {
        enemy.Body.velocity = new Vector2(0, enemy.Body.velocity.y); // Просто держим скорость по X равной 0, чтобы враг стоял на месте и не толкал игрока
    }
}