using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushPullObject : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = .15f;
    [SerializeField] protected LayerMask whatIsGround;
    private bool isInteracting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ba�lang��ta yatayda kilitli / tamamen donuk olabilir
        // �rnek: Y ekseni serbest, X donuk + rotasyon donuk
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // ya da diledi�in �ekilde
    }

    private void FixedUpdate()
    {
        if (!isInteracting && IsGroundDetected())
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void SetInteracting(bool value)
    {
        if (value)
        {
            isInteracting = true;
            // E tu�una bas�ld���nda objeyi serbest b�rak
            rb.constraints = RigidbodyConstraints2D.None;
            // istersen rotasyon sabit:
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            isInteracting = false;
            if (IsGroundDetected())
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}
