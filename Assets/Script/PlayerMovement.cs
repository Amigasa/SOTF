using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //��������� ������ � ������� ������� PlayerData
    public PlayerData Data;

    #region ����������
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region ���������
    //���������� ��������� ���������� ����������, ������� ����� ����� ��������� � ����� �����.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }

    //�������
    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    //������
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //������ �� �����
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    //���
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;

    #endregion

    #region ������� ���������
    private Vector2 _moveInput;

    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    #endregion

    #region �������� ����������
    //��������� ��� ��� � ����������
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    //������ �������� �� ����� ������� �� ������� ������ ���������, ��� �������, �� ������, ����� ��� ���� ������� ������ ������ (��� �������� �� �����) � ������ (��� �������� �� �����).
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    #region ���� � ����
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
        #region �������
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        #endregion

        #region ��������� ������� ������
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space)) //������ ��� �������
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space)) //��� ����������
        {
            OnJumpUpInput();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) //����� ��� �������
        {
            OnDashInput();
        }
        #endregion

        #region �������� �� ������������
        if (!IsDashing && !IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //���������, �� ������������� �� ������������ ���� � ������
            {

                LastOnGroundTime = Data.coyoteTime; //���� ��� ���, ������������� �������� lastGrounded ������ coyoteTime
            }

            //�������� ������ �����
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
                LastOnWallRightTime = Data.coyoteTime;

            //�������� ������ �����
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
                LastOnWallLeftTime = Data.coyoteTime;

            //��� �������� ���������� ��� ��� �����, ��� � ��� ������ �����, ��� ��� ��� ������ �������� ���� ����������� ����� �� ����� �������� �������
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region �������� ������
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
            //������
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }
            //������ �� �����
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

        #region �������� �����
        if (CanDash() && LastPressedDashTime > 0)
        {
            //������������ ���� �� ���� �������. ��� ������� ���� �������� � ������� �������� ��������������.
            Sleep(Data.dashSleepTime);

            //���� ����������� ������ �� ����, ��������� ����� ������
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

        #region �������� ���������� �� �����
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
        #endregion

        #region ����������
        if (!_isDashAttacking)
        {
            //����� ������� ����������, ���� �� ��������� ������ ������ ��� ������
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                //������� ������� ���� ������� ��� �����������
                SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
                //������������ ������������ �������� �������, ������� ��� ������� �� ������� ���������� �� �� ����������� �� ������� ������� ���������
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                //���� ������� ����, ���� ��������� ������ ������
                SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
            {
                SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                //����� ������� ���� ������� ��� �������
                SetGravityScale(Data.gravityScale * Data.fallGravityMult);
                //������������ ������������ �������� �������, ������� ��� ������� �� ������� ���������� �� �� ����������� �� ������� ������� ���������
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else
            {
                //���� ������� �� ��������� ��� ������� �� ��������� ��� �������� �����
                SetGravityScale(Data.gravityScale);
            }
        }
        else
        {
            //���������� ���������� ��� ����� (������������ � ���������� ��������� ����� ���������� ��������� ���� �������� �����)
            SetGravityScale(0);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        //���
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
        
        //���������� �� �����
        if (IsSliding)
            Slide();
    }

    #region �������� ������ �����
    //������, ������� ������������ ����, ������������ � Update()
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

    #region ����� ������
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        //����� ������������ ��� ����, ����� ��� �� ����� ���� �������� StartCoroutine �����
        //����������� nameof() ��������, ��� ��� �� ����� ������� ������ ��������.
        //��������� ����������� ��������������� ������ � �������� �������� ��������� �� �������, ���� ������� �������
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration); //������ ���� � �������� �������, ������� �� ����� �������, ������ 0
        Time.timeScale = 1;
    }
    #endregion

    #region ������ ������������
    private void Run(float lerpAmount)
    {
        //����������� �����������, � ������� �� ����� ���������, � �������� ��������
        float targetSpeed = _moveInput.x * Data.runMaxSpeed;
        //�� ����� ��������� ���������� � ������� Lerp(), ��� ���������� ��������� ����������� � ��������
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region ��������� ���������
        float accelRate;

        //�������� �������� ���������, ���������� �� ���, ���������� �� �� (������� �������) 
        //��� �������� ����������� (������������). � ����� ��������� ���������, ���� �� �������� �� �������.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region ���������� ��������� ��������� �� ������� ������
        //���������� ��������� � ������������ �������� �� ���� ������ ������ ��� ����� �������, ���������� � ������������
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }
        #endregion

        #region ���������� ��������
        //�� �� ����� ��������� ������, ���� �� �������� � ������ �����������, �� �� ���������, ����������� ��� ������������ ��������
        if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //������������� ����� ���������� ���, ������� �������, ��������� ������� ��������
            accelRate = 0;
        }
        #endregion

        //��������� ������� ����� ������� ��������� � �������� ���������
        float speedDif = targetSpeed - RB.velocity.x;

        //����������� ���� ����� ��� x, ������� ���������� ��������� � ������
        float movement = speedDif * accelRate;

        //������������ ��� �������� � ������ � ��������� � �������� ����
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        //��������� ������� � ������������ ������������� �� ��� x
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region ������ ������
    private void Jump()
    {
        //�����������, ��� �� �� ������ ������� ������ ��������� ��� ����� ��������
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;


        #region ��������� ������
        //�� ����������� ����, ����������� ��� �������
        //��� ��������, ��� ��� ������ ����� ��������, ��� �� ������� �� ���������� �������� 
        float force = Data.jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    private void WallJump(int dir)
    {
        //�����������, ��� �� �� ������ ������� ������ �� ����� ��������� ��� ����� ��������
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region ��������� ������ �� �����
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //��������� ������ � ��������������� ����������� �� �����

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //���������, ������ �� �����, ���� ��� ���, �� �������� velocity.y (������������������ ���� �������). ��� �����������, ��� ����� ������ ��������� �������� ���� ������ ��� ������
            force.y -= RB.velocity.y;

        //� ������� �� ����, �� ����� ������������ Impulse �����.
        //�� ��������� ����� ����������� default mode, ��������� ������������ �������� ��������.
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region ������ ����
    //����������� ����
    private IEnumerator StartDash(Vector2 dir)
    {

        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        //�� ��������� �������� ������ �� ������ �������� ����� �� ����� ���� "�����"
        while (Time.time - startTime <= Data.dashAttackTime)
        {
            RB.velocity = dir.normalized * Data.dashSpeed;
            //���������������� ���� �� ���������� �����, �������� ���-�� ����� ����� ����������. 
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        //���������� "�����" ������ ����, ��� �� ���������� ������ ����� ����������, �� ��-�������� ������������ ��������� ���� (��. Update() � Run()).
        SetGravityScale(Data.gravityScale);
        RB.velocity = Data.dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= Data.dashEndTime)
        {
            yield return null;
        }

        //����� ������
        IsDashing = false;
    }

    //�������� ���������� ������� �� ����, ��� ����� ������ ����� ��������� �����
    private IEnumerator RefillDash(int amount)
    {
        //����� ����������� ���������, ������� �� �� ����� ��������� ������ �� �����
        _dashRefilling = true;
        yield return new WaitForSeconds(Data.dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
    }
    #endregion

    #region ������ ������� ������������
    private void Slide()
    {
        //�� ��������� ���������� ������� �������� �����, ����� ������������� ���������� �����
        if (RB.velocity.y > 0)
        {
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        //�������� ��� ��, ��� � ���, �� ������ �� ��� y
        float speedDif = Data.slideSpeed - RB.velocity.y;
        float movement = speedDif * Data.slideAccel;
        //����, �� ������������ �������� �����, ����� ������������� �����-���� ���������� ������������� (��� �� ������� ��� �������)
        //����������� ������ �� ����� ��������� (�������������) ������� � �������� * �� ������� ���, ������� ��� ���������� ������ ������� FixedUpdate()
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion


    #region ������ ��������
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


    #region ������ ��������������
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

