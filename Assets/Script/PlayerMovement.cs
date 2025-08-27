using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Параметры игрока с помощью скрипта PlayerData
    public PlayerData Data;

    #region Компоненты
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region Параметры
    //Переменные управляют различными действиями, которые игрок может выполнить в любое время.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }

    //Таймеры
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //Прыжок
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Прыжок от стены
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    //Дэш
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;

    #endregion

    #region Входные параметры
    private Vector2 _moveInput;

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    #endregion

    #region Проверка параметров
    //Настроить все это в инспекторе
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //Размер проверки на земле зависит от размера вашего персонажа, как правило, вы хотите, чтобы они были немного меньше ширины (для проверки на земле) и высоты (для проверки на стене).
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    #region Слои и теги
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        #region Таймеры
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        #endregion

        #region Обработка входных данных
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space)) //Прыжок при нажатии
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space)) //При отпускании
        {
            OnJumpUpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) //Рывок при нажатии
        {
            OnDashInput();
        }
        #endregion

        #region Проверка на столкновение
        if (!IsDashing && !IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //проверяет, не перекрывается ли установочный блок с землей
            {

                LastOnGroundTime = Data.coyoteTime; //если это так, устанавливает значение lastGrounded равным coyoteTime
            }

            //Проверка правой стены
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
                LastOnWallRightTime = Data.coyoteTime;

            //Проверка правой стены
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
                LastOnWallLeftTime = Data.coyoteTime;

            //Две проверки необходимы как для левой, так и для правой стены, так как при каждом повороте игры контрольные точки на стене меняются местами
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region Проверка прыжка
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

            _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            //Прыжок
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            //Прыжок от стены
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;

                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

                WallJump(_lastWallJumpDir);
            }
        }
        #endregion

        #region Проверка рывка
        if (CanDash() && LastPressedDashTime > 0)
        {
            //Замораживает игру на долю секунды. Это придает игре сочности и немного смягчает направленность.
            Sleep(Data.dashSleepTime);

            //Если направление нажато не было, выполнить рывок вперед
            if (_moveInput != Vector2.zero)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;



            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(StartDash), _lastDashDir);
        }
        #endregion

        #region Проверка скольжения по стене
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
        #endregion

        #region Гравитация
        if (!_isDashAttacking)
        {
            //Более высокая гравитация, если мы отпустили кнопку прыжка или падаем
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //Гораздо большая сила тяжести при удерживании
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //Ограничивает максимальную скорость падения, поэтому при падении на большие расстояния мы не разгоняемся до безумно высоких скоростей
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //Сила тяжести выше, если отпустить кнопку прыжка
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //Более высокая сила тяжести при падении
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //Ограничивает максимальную скорость падения, поэтому при падении на большие расстояния мы не разгоняемся до безумно высоких скоростей
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //Сила тяжести по умолчанию при стоянии на платформе или движении вверх
                SetGravityScale(Data.gravityScale);
            }
        }
        else
        {
            //Отсутствие гравитации при рывке (возвращается в нормальное состояние после завершения начальной фазы рывковой атаки)
            SetGravityScale(0);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        //Бег
        if (!IsDashing)
        {
            if (IsWallJumping)
                Run(Data.wallJumpRunLerp);
            else
                Run(1);
        }
        else if (_isDashAttacking)
        {
            Run(Data.dashEndRunLerp);
        }
        
        //Скольжение по стене
        if (IsSliding)
            Slide();
    }

    #region Обратные методы ввода
    //Методы, которые обрабатывают ввод, обнаруженные в Update()
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }

    public void OnDashInput()
    {
        LastPressedDashTime = Data.dashInputBufferTime;
    }
    #endregion

    #region Общие методы
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        //Метод используется для того, чтобы нам не нужно было вызывать StartCoroutine везде
        //обозначение nameof() означает, что нам не нужно вводить строку напрямую.
        //Устраняет вероятность орфографических ошибок и улучшает качество сообщений об ошибках, если таковые имеются
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //Должно быть в реальном времени, начиная со шкалы времени, равной 0
        Time.timeScale = 1;
    }
    #endregion

    #region Методы передвижения
    private void Run(float lerpAmount)
    {
        //Рассчитайте направление, в котором мы хотим двигаться, и желаемую скорость
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //Мы можем уменьшить управление с помощью Lerp(), это сглаживает изменения направления и скорости
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Вычислить ускорение
        float accelRate;

        //Получает значение ускорения, основанное на том, ускоряемся ли мы (включая поворот) 
        //или пытаемся замедлиться (остановиться). А также применяет множитель, если мы движемся по воздуху.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Добавление бонусного ускорения на вершине прыжка
        //Увеличение ускорения и максимальной скорости на пике прыжка делает его более упругим, отзывчивым и естественным
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Сохранение импульса
        //Мы не будем замедлять игрока, если он движется в нужном направлении, но со скоростью, превышающей его максимальную скорость
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Предотвратить любое замедление или, другими словами, сохранить текущую динамику
            accelRate = 0;
        }
        #endregion

        //Вычислить разницу между текущей скоростью и желаемой скоростью
        float speedDif = targetSpeed - RB.velocity.x;

        //Рассчитайть силу вдоль оси x, которую необходимо приложить к игроку
        float movement = speedDif * accelRate;

        //Преобразуйте это значение в вектор и примените к твердому телу
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        //сохраняет масштаб и поворачивает проигрыватель по оси x
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region Методы прыжка
    private void Jump()
    {
        //Гарантирует, что мы не сможем сделать прыжок несколько раз одним нажатием
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;


        #region Выполнить прыжок
        //Мы увеличиваем силу, прилагаемую при падении
        //Это означает, что нам всегда будет казаться, что мы прыгаем на одинаковую величину 
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    private void WallJump(int dir)
    {
        //Гарантирует, что мы не сможем сделать прыжок от стены несколько раз одним нажатием
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Выполнить прыжок от стены
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //приложить усилие в противоположном направлении от стены

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //проверяет, падает ли игрок, если это так, мы вычитаем velocity.y (противодействующая сила тяжести). Это гарантирует, что игрок всегда достигнет желаемой силы прыжка или больше
            force.y -= RB.velocity.y;

        //В отличие от бега, мы хотим использовать Impulse режим.
        //По умолчанию будет применяться default mode, мгновенно игнорирующий массовую нагрузку.
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region Методы дэша
    //Сопрограмма дэша
    private IEnumerator StartDash(Vector2 dir)
    {

        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        //Мы сохраняем скорость игрока на уровне скорости рывка во время фазы "атака"
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            //Приостанавливает цикл до следующего кадра, создавая что-то вроде цикла обновления. 
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        //Начинается "конец" нашего дэша, где мы возвращаем игроку часть управления, но по-прежнему ограничиваем ускорение бега (см. Update() и Run()).
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        //Рывок вперед
        IsDashing = false;
    }

    //Короткий промежуток времени до того, как игрок сможет снова совершить рывок
    private IEnumerator RefillDash(int amount)
    {
        //Время перезарядки выключено, поэтому мы не можем постоянно бегать по земле
        _dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
    }
    #endregion

    #region Другие способы передвижения
    private void Slide()
    {
        //Мы устраняем оставшийся импульс движения вверх, чтобы предотвратить скольжение вверх
        if (RB.velocity.y > 0)
        {
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        //Работает так же, как и бег, но только по оси y
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;
        //Итак, мы ограничиваем движение здесь, чтобы предотвратить какие-либо чрезмерные корректировки (они не заметны при запуске)
        //Приложенное усилие не может превышать (отрицательную) разницу в скорости * на столько раз, сколько раз вызывается вторая функция FixedUpdate()
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion


    #region Методы проверки
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();

    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.velocity.y > 0;
    }

    private bool CanDash()
    {
        if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }
    #endregion


    #region Методы редактирования
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}

