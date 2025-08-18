using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] //Создайть новый объект PlayerData, щелкнув правой кнопкой мыши в меню Project, затем Create/Player/Player Data и перетащить на плеер
public class PlayerData : ScriptableObject
{
	[Header("Gravity")]
	[HideInInspector] public float gravityStrength; //Направленное вниз усилие (сила тяжести), необходимое для достижения желаемой высоты и времени прыжка.
    [HideInInspector] public float gravityScale; //Сила притяжения игрока как множитель гравитации (устанавливается в ProjectSettings/Physics2D).
                                                 //Также задано значение шкалы гравитации rigidbody2D.gravityScale
    [Space(5)]
	public float fallGravityMult; //Множитель к шкале гравитации игрока при падении.
    public float maxFallSpeed; //Максимальная скорость падения (предельная скорость) игрока при падении.
    [Space(5)]
	public float fastFallGravityMult; //Увеличенный коэффициент к шкале гравитации игрока, когда он падает и нажата клавиша "Вниз".
    public float maxFastFallSpeed; //Максимальная скорость падения игрока при выполнении более быстрого падения.

    [Space(20)]

	[Header("Run")]
	public float runMaxSpeed; //Целевая скорость, которую мы хотим, чтобы достиг игрок.
    public float runAcceleration; //Скорость, с которой наш игрок разгоняется до максимальной скорости, может быть установлена на значение runMaxSpeed для мгновенного ускорения до 0 или вообще без него
    [HideInInspector] public float runAccelAmount; //Фактическая сила (умноженная на разницу в скорости), приложенная к игроку.
    public float runDecceleration; //Скорость, с которой наш игрок замедляется по сравнению со своей текущей скоростью, может быть установлена на значение runMaxSpeed для мгновенного замедления или на 0 для полного отсутствия
    [HideInInspector] public float runDeccelAmount; //Фактическая сила (умноженная на разницу в скорости), приложенная к игроку
    [Space(5)]
	[Range(0f, 1)] public float accelInAir; //Множители, применяемые к скорости ускорения в воздухе.
    [Range(0f, 1)] public float deccelInAir;
	[Space(5)]
	public bool doConserveMomentum = true;

	[Space(20)]

	[Header("Jump")]
	public float jumpHeight; //Высота прыжка игрока
    public float jumpTimeToApex; //Время между применением силы прыжка и достижением желаемой высоты прыжка. Эти значения также определяют силу тяжести игрока и силу прыжка.
    [HideInInspector] public float jumpForce; //Фактическая сила, приложенная (вверх) к игроку при прыжке.

    [Header("Both Jumps")]
	public float jumpCutGravityMult; //Множитель, увеличивающий силу тяжести, если игрок отпускает кнопку прыжка, продолжая прыгать
    [Range(0f, 1)] public float jumpHangGravityMult; //Уменьшает силу тяжести, находясь близко к вершине (желаемой максимальной высоте) прыжка
    public float jumpHangTimeThreshold; //Скорости (близкие к 0), при которых игрок будет испытывать дополнительное "зависание в прыжке". Скорость игрока.значение y ближе всего к 0 в точке вершины прыжка (представьте себе градиент параболы или квадратичной функции).
    [Space(0.5f)]
	public float jumpHangAccelerationMult; 
	public float jumpHangMaxSpeedMult; 				

	[Header("Wall Jump")]
	public Vector2 wallJumpForce; //Фактическая сила (на этот раз установленная нами), приложенная к игроку при прыжке через стену.
    [Space(5)]
	[Range(0f, 1f)] public float wallJumpRunLerp; //Уменьшает эффект передвижения игрока при прыжках по стене.
    [Range(0f, 1.5f)] public float wallJumpTime; //На какое время после прыжка через стену движение игрока замедляется.
    public bool doTurnOnWallJump; //Игрок повернется лицом к стене в направлении прыжка

    [Space(20)]

	[Header("Slide")]
	public float slideSpeed;
	public float slideAccel;

    [Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime; //Льготный период после падения с платформы, когда вы все еще можете прыгнуть
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Льготный период после нажатия кнопки прыжка, в течение которого прыжок будет выполнен автоматически после выполнения требований (например, при заземлении).

    [Space(20)]

	[Header("Dash")]
	public int dashAmount;
	public float dashSpeed;
	public float dashSleepTime; //Продолжительность, на которую игра зависает, когда мы нажимаем рывок, но до того, как мы прочитаем направление ввода и приложим усилие
    [Space(5)]
	public float dashAttackTime;
	[Space(5)]
	public float dashEndTime; //Время после завершения начальной фазы рывка, сглаживающей переход обратно в стандартный режим
    public Vector2 dashEndSpeed; //Замедляет работу игрока, делает рывок более отзывчивым
    [Range(0f, 1f)] public float dashEndRunLerp; //Замедляет эффект передвижения игрока во время рывка
    public float dashRefillTime;
	[Space(5)]
	[Range(0.01f, 0.5f)] public float dashInputBufferTime;


    //Обратный вызов Unity, вызываемый при обновлении инспектора
    private void OnValidate()
    {
        //Рассчитайте силу тяжести по формуле (gravity = 2 * jumpHeight / timeToJumpApex^2)
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Вычислить масштаб силы тяжести твердого тела (т.е. силу тяжести относительно значения силы тяжести unity, см. project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Рассчитайте силы ускорения и замедления при движении по формуле: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Рассчитайте силу прыжка по формуле (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Изменяемые диапазоны
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		#endregion
	}
}