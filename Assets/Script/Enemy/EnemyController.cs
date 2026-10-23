using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Анимации")]
    public Animator Anim;

    [Header("Данные и Сенсор")]
    public EnemyData Data;
    public EnemySensor Sensor;

    public Rigidbody2D Body { get; private set; }
    public StateMachine Machine { get; private set; }
    public bool IsDangerous { get; set; } = false;

    // --- ИЗМЕНЕНО ДЛЯ HFSM ---
    // Теперь мы храним только корневые состояния
    public AliveState AliveState { get; private set; }
    public DeadState DeadState { get; private set; }

    [Header("Здоровье")]
    public int health = 3;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        if (Sensor == null) Sensor = GetComponent<EnemySensor>();
        if (Sensor != null) Sensor.data = Data;

        Machine = new StateMachine();

        // Создаем только корневые состояния
        AliveState = new AliveState(this, Machine);
        DeadState = new DeadState(this, Machine);
        Anim = GetComponent<Animator>(); // Анимация
    }

    private void Start()
    {
        // Стартуем с состояния "Живой"
        Machine.ChangeState(AliveState);
    }

    private void Update()
    {
        Machine.Tick();
    }

    private void FixedUpdate()
    {
        Machine.PhysicsTick();
    }

    public void FaceDirection(int dir)
    {
        if (dir == 0) return;
        Vector3 s = transform.localScale;
        s.x = -Mathf.Abs(s.x) * Mathf.Sign(dir);
        transform.localScale = s;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // В HFSM мы сразу переключаем корневое состояние на Смерть
            Machine.ChangeState(DeadState);
        }
    }

    public void PerformAttack()
    {
        if (Sensor.Player == null) return;
        if (Vector2.Distance(transform.position, Sensor.Player.position) <= Data.attackRadius)
        {
            var hp = Sensor.Player.GetComponent<HudHP>();
            if (hp != null) hp.ApplyDamage(Data.attackDamage);
        }
    }

    public void PlayDeathAnim()
    {
        if (Anim != null)
        {
            Anim.SetBool("IsDead", true); // Включаем анимацию смерти
        }
    }
}