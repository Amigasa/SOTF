using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Для удобства, если повесить скрипт на пустой объект, сразу создаст ригитбади
public class EnemyController : MonoBehaviour
{
    [Header("Данные и Сенсор")]
    public EnemyData Data;
    public EnemySensor Sensor;

    public Rigidbody2D Body { get; private set; }
    public StateMachine Machine { get; private set; }

    // Опасен ли враг прямо сейчас
    public bool IsDangerous { get; set; } = false;

    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public DeadState DeadState { get; private set; }

    [Header("Здоровье")]
    public int health = 3;

    private void Awake() // До старта игры получаем компоненты
    {
        Body = GetComponent<Rigidbody2D>();
        if (Sensor == null) Sensor = GetComponent<EnemySensor>();
        if (Sensor != null) Sensor.data = Data;

        Machine = new StateMachine();

        PatrolState = new PatrolState(this, Machine);
        ChaseState = new ChaseState(this, Machine);
        AttackState = new AttackState(this, Machine);
        DeadState = new DeadState(this, Machine);
    }

    private void Start() // При старте начинаем с патруля
    {
        Machine.ChangeState(PatrolState);
    }

    private void Update() // Каждый кадр передает команду тик
    {
        Machine.Tick();
    }

    private void FixedUpdate() // Для физики
    {
        Machine.PhysicsTick(); // Для движения если точнее
    }

    public void FaceDirection(int dir) // Переворачивает спрайт, а то до этого он спиной на игрока нападал)
    {
        if (dir == 0) return;
        Vector3 s = transform.localScale;
        s.x = -Mathf.Abs(s.x) * Mathf.Sign(dir); // Возвращает 1 (если идем вправо) или -1 (если влево). Минус перед Mathf.Abs нужен, чтобы спрайт смотрел туда, куда идет.
        transform.localScale = s;
    }

    public void TakeDamage(int damage) // Отнимает здоровье
    {
        health -= damage;
        if (health <= 0) // Если хп 0
        {
            Machine.ChangeState(DeadState); // То смеееертт
        }
    }

    public void PerformAttack() // Ищет на игроке скрипт HudHP и вызывает у него ApplyDamage
    {
        if (Sensor.Player == null) return;
        if (Vector2.Distance(transform.position, Sensor.Player.position) <= Data.attackRadius)
        {
            var hp = Sensor.Player.GetComponent<HudHP>();
            if (hp != null) hp.ApplyDamage(Data.attackDamage);
        }
    }
}