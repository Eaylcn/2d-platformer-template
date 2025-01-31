using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy { get; private set; }
    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool isLanded;
    [HideInInspector] public bool isRolling;
    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool isWalking;
    public bool isOnLadder;

    public float normalGravity { get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    [HideInInspector] public int hurtCount;

    [Header("Move Info")]
    public float runSpeed = 7f;
    public float walkSpeed = 3f;
    public float crouchWalkSpeed = 4f;

    [Header("Jump Info")]
    public float jumpForce = 12f;
    public float wallJumpForce = 12f;

    [Header("Roll Info")]
    public float rollSpeed = 25f;
    public float rollDuration = 0.2f;
    [HideInInspector] public float rollDir;

    [Header("Slide Info")]
    public float slideSpeed = 25f;
    public float slideDuration = 0.2f;
    [HideInInspector] public float slideDir;

    [Header("Push Pull Info")]
    public float pushSpeed = 3f;
    public float pullSpeed = 3f;

    [Header("Climb Info")]
    public float climbSpeed = 3f;

    [Header("Collider Settings")]
    private Vector2 standSize = new Vector2(0.85f, 1.84f);
    private Vector2 rollSize = new Vector2(1f, 1f);
    private Vector2 standColliderPosition = new Vector2(-0.05f, -0.07f);
    private Vector2 rollColliderPosition = new Vector2(-0.1f, -0.5f);
    private CapsuleCollider2D mainCollider;

    [Header("Wall Check Positions")]
    private Vector2 defaultWallCheckLocalPos;
    private Vector2 crouchingWallCheckLocalPos = new Vector2(0.4f, -0.35f);


    [Header("Check References")]
    public Transform handAnchor;
    [SerializeField] private Transform pushPullCheckPoint;
    [SerializeField] protected Transform crouchTopFloorCheck;
    private float pushPullCheckRadius = 0.1f;
    protected float crouchTopFloorCheckDistance = 0.15f;
    [SerializeField] private LayerMask whatIsPushPull;
    [SerializeField] private LayerMask whatIsLadder;

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
    public PlayerPushPullIdleState pushPullIdleState { get; private set; }
    public PlayerPushState pushState { get; private set; }
    public PlayerPullState pullState { get; private set; }
    public PlayerPunchState punchAttack { get; private set; }
    public PlayerRollState rollState { get; private set; }
    public PlayerSlideState slideState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerHurtState hurtState { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    public PlayerLadderClimbState ladderClimbState { get; private set; }
    public PlayerLadderDownState ladderDownState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        mainCollider = GetComponent<CapsuleCollider2D>();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        runState = new PlayerRunState(this, stateMachine, "Run");
        walkState = new PlayerWalkState(this, stateMachine, "Walk");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        landState = new PlayerLandState(this, stateMachine, "Land");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        crouchWalkState = new PlayerCrouchWalkState(this, stateMachine, "CrouchWalk");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
        pushPullIdleState = new PlayerPushPullIdleState(this, stateMachine, "PushPullIdle");
        pushState = new PlayerPushState(this, stateMachine, "Push");
        pullState = new PlayerPullState(this, stateMachine, "Pull");
        punchAttack = new PlayerPunchState(this, stateMachine, "Punch");
        rollState = new PlayerRollState(this, stateMachine, "Roll");
        slideState = new PlayerSlideState(this, stateMachine, "Slide");
        hurtState = new PlayerHurtState(this, stateMachine, "Hurt");
        climbState = new PlayerClimbState(this, stateMachine, "Climb");
        ladderClimbState = new PlayerLadderClimbState(this, stateMachine, "LadderClimb");
        ladderDownState = new PlayerLadderDownState(this, stateMachine, "LadderDown");
    }

    protected override void Start()
    {
        base.Start();

        defaultWallCheckLocalPos = wallCheck.localPosition;
        normalGravity = rb.gravityScale;
        isWalking = false;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();
        ChangeMoveMode();
        PlayerHurtCheck();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void PlayerHurtCheck()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(hurtState);
        }
    }

    public void ChangeMoveMode()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
            isWalking = !isWalking;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void ClearModes()
    {
        if (isCrouching)
            isCrouching = false;
        if (isRolling)
            isRolling = false;
        if (isSliding)
            isSliding = false;
        if (isOnLadder)
            isOnLadder = false;
    }

    public PushPullObject CheckForPushPullObject()
    {
        if (pushPullCheckPoint == null)
            return null;

        Vector2 circleCenter = pushPullCheckPoint.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(circleCenter, pushPullCheckRadius, whatIsPushPull);

        foreach (var hit in hits)
        {
            PushPullObject ppo = hit.GetComponent<PushPullObject>();
            if (ppo != null)
                return ppo;
        }
        return null;
    }

    public void UpdateWallCheckPosition()
    {
        wallCheck.localPosition = isCrouching || isRolling || isSliding ? crouchingWallCheckLocalPos : defaultWallCheckLocalPos;
    }

    #region Collision

    public void ColliderUpdate()
    {

        mainCollider.enabled = true;

        var (size, offset) = (standSize, standColliderPosition);

        if (isRolling) (size, offset) = (rollSize, rollColliderPosition);
        else if (isCrouching) (size, offset) = (rollSize, rollColliderPosition);
        else if (isSliding) (size, offset) = (rollSize, rollColliderPosition);


        mainCollider.size = size;
        mainCollider.offset = offset;


        if (isOnLadder)
            mainCollider.enabled = false;

        // Entity'deki wall check pozisyonunu güncelle
        UpdateWallCheckPosition();
    }

    public bool IsCrouchTopFloorDetected()
    {
        return Physics2D.BoxCast(
            crouchTopFloorCheck.position,
            new Vector2(0.5f, 0.5f), // Boyutu ayarlayýn
            0,
            Vector2.up,
            crouchTopFloorCheckDistance,
            whatIsGround
        );
    }

    public bool IsLadderTopDetected()
    {
        return Physics2D.BoxCast(
            crouchTopFloorCheck.position,
            new Vector2(0.5f, 0.5f), // Boyutu ayarlayýn
            0,
            Vector2.up,
            crouchTopFloorCheckDistance,
            whatIsLadder
        );
    }

    public virtual bool IsLadderBottomDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheck.position,
            groundCheckSize,
            0,
            Vector2.down,
            groundCheckDistance,
            whatIsLadder
        );
        return hit.collider != null;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.magenta;
        Vector3 boxCenter = crouchTopFloorCheck.position + Vector3.up * crouchTopFloorCheckDistance;
        Gizmos.DrawWireCube(boxCenter, new Vector3(0.5f, 0.5f));

        if (pushPullCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pushPullCheckPoint.position, pushPullCheckRadius);
        }
    }

    #endregion
}
