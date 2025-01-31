using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    [SerializeField] protected float wallCheckDistance = 0.8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update() { }

    #region Velocity
    public void SetZeroVelocity() => rb.velocity = Vector2.zero;

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetVelocityNoFlip(float x, float y) => rb.velocity = new Vector2(x, y);
    #endregion

    #region Collision
    public virtual bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheck.position,
            groundCheckSize,
            0,
            Vector2.down,
            groundCheckDistance,
            whatIsGround
        );
        return hit.collider != null;
    }

    public virtual bool IsWallDetected()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            wallCheck.position,
            wallCheckSize,
            0,
            Vector2.right * facingDir,
            wallCheckDistance,
            whatIsGround
        );
        return hit.collider != null;
    }

    protected virtual void OnDrawGizmos()
    {
        // Ground Check Gizmos
        Gizmos.color = Color.green;
        Vector3 groundBoxCenter = groundCheck.position + Vector3.down * groundCheckDistance;
        Gizmos.DrawWireCube(groundBoxCenter, new Vector3(groundCheckSize.x, groundCheckSize.y));

        // Wall Check Gizmos
        Gizmos.color = Color.blue;
        Vector3 wallBoxCenter = wallCheck.position + (Vector3)(Vector2.right * facingDir * wallCheckDistance);
        Gizmos.DrawWireCube(wallBoxCenter, new Vector3(wallCheckSize.x, wallCheckSize.y));
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        onFlipped?.Invoke();
    }

    public virtual void SetupDefaultFacingDirection(int facingDir)
    {
        this.facingDir = facingDir;
        facingRight = facingDir == 1;
    }

    public virtual void FlipController(float xInput)
    {
        if ((xInput > 0 && !facingRight) || (xInput < 0 && facingRight))
            Flip();
    }
    #endregion
}