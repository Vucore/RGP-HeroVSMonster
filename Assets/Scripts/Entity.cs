using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Compoments
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    #endregion
    
    
    [Header("Collision")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float cirleGroundCheckRadius;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("KnockBack")]
    [SerializeField] protected Vector2 knockBackDirection;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnockBack;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    protected virtual void Awake() 
    {
        
    }
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();

    }
    protected virtual void Update() 
    {

    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnockBack)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFx");
        StartCoroutine("HitKnockBack");
    }
    protected virtual IEnumerator HitKnockBack()
    {
        isKnockBack = true;
        rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnockBack = false;


    }
    public void MakeTransparent(bool _transparent)
    {
        if(_transparent)
            sr.color = Color.clear;
        else 
            sr.color = Color.white;

    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   // public virtual bool IsGroundDetected() => Physics2D.OverlapCircle(groundCheck.position, cirleGroundCheckRadius, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    
    protected virtual void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawWireSphere(groundCheck.position, cirleGroundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));   
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion


    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipController(float _x)
    {
        if(_x > 0 && !facingRight)
        {
            Flip();
        }
        else if(_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion

}
