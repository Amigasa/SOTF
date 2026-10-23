using UnityEngine;

public class ChaseState : EnemyStateBase
{
    public ChaseState(EnemyController e, StateMachine m) : base(e, m) { }

    public override void Enter()
    {
        enemy.IsDangerous = true; // В погоне враг опасен 0_0 и агрессивен)
    }

    public override void PhysicsTick() // Проверяем, есть ли игрок
    {
        if (enemy.Sensor.Player == null) return;

        // Берем координату X игрока, вычитаем координату X врага.
        float dir = Mathf.Sign(enemy.Sensor.Player.position.x - enemy.transform.position.x); // Mathf.Sign превращает это число в 1 (если игрок справа) или -1 (если слева).
        enemy.Body.velocity = new Vector2(dir * enemy.Data.chaseSpeed, enemy.Body.velocity.y); // Затем умножаем на скорость погони и применяем к Rigidbody2D.
        enemy.FaceDirection((int)dir); // Вызов метода для переворота спрайта врага
        enemy.Anim.SetFloat("Speed", Mathf.Abs(enemy.Body.velocity.x));
    }
}