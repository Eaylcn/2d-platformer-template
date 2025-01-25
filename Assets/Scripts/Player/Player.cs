using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    public bool isCrouching;

    [Header("Move Info")]
    public float runSpeed = 7f;
    public float jumpForce = 12f;
    public float wallJumpForce = 12f;
    public float crouchWalkSpeed = 4f;

    [Header("Crouch Settings")]
    [SerializeField] private Vector2 standingSize = new Vector2(0.85f, 1.84f);
    [SerializeField] private Vector2 crouchingSize = new Vector2(1f, 1f);
    [SerializeField] private Vector2 standingColliderPosition = new Vector2(-0.05f, -0.07f);
    [SerializeField] private Vector2 crouchingColliderPosition = new Vector2(0f, -0.5f);
    private CapsuleCollider2D mainCollider;

    [Header("Check References")]
    [SerializeField] protected Transform crouchTopFloorCheck;
    [SerializeField] protected Vector2 defaultWallCheckPosition;
    [SerializeField] protected Vector2 crouchingWallCheckPosition;
    [SerializeField] protected float crouchTopFloorCheckDistance = .8f;

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerCrouchWalkState crouchWalkState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        mainCollider = GetComponent<CapsuleCollider2D>();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        runState = new PlayerRunState(this, stateMachine, "Run");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        landState = new PlayerLandState(this, stateMachine, "Land");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        crouchWalkState = new PlayerCrouchWalkState(this, stateMachine, "CrouchWalk");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();
        ColliderUpdate();
        WallCheckPositionUpdate();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    #region Collision

    public void ColliderUpdate()
    {
        mainCollider.size = isCrouching ? crouchingSize : standingSize;
        mainCollider.offset = isCrouching ? crouchingColliderPosition : standingColliderPosition;
    }

    public void WallCheckPositionUpdate()
    {
        wallCheck.localPosition = isCrouching ? crouchingWallCheckPosition : defaultWallCheckPosition;
    }

    public virtual bool IsCrouchTopFloorDetected() => Physics2D.Raycast(crouchTopFloorCheck.position, Vector2.up, groundCheckDistance, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(crouchTopFloorCheck.position, new Vector2(crouchTopFloorCheck.position.x, crouchTopFloorCheck.position.y + crouchTopFloorCheckDistance));
    }

    #endregion
}
