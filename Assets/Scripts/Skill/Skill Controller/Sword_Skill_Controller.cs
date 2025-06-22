using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimerDuration;
    private float returnSwordSpeed = 12f;

    [Header("Pierce")]
    private int pierceAmount;

    [Header("Bounce")]
    [SerializeField] private float bouncingRadius;
    private float bouncingSpeed;
    private bool isBouncing;
    private int bounceAmount;

    [Header("Spin")]
    [SerializeField] private float spinningRadius;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float spinDirection;
    private float hitTimer;
    private float hitCooldown;


    private List<Transform> enemyTarget;
    private int enemyTargerIndex;
    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    public void SetUpSword(Vector2 _dir, float _gravity, Player _player, float _freezeTimeDuration,float _returnSwordSpeed)
    {
        rb.gravityScale = _gravity;
        rb.velocity = _dir;
        player = _player;
        freezeTimerDuration = _freezeTimeDuration;
        returnSwordSpeed = _returnSwordSpeed;



        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        Invoke("DestroyMe", 7);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
    }
    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bouncingSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    public void ReturnSword()
    {
        anim.SetBool("Rotation", true);
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSwordSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                Debug.Log(spinDirection);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1f * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinningRadius);
                    foreach (Collider2D hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[enemyTargerIndex].position, bouncingSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[enemyTargerIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[enemyTargerIndex].GetComponent<Enemy>());
                enemyTargerIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
            }
            if (enemyTargerIndex >= enemyTarget.Count)
            {
                enemyTargerIndex = 0;
            }
        }
    }

    
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bouncingRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spinningRadius);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (isReturning)
            return;
        if(other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetForBounce(other);

        StuckInto(other);
    }


    private void SetupTargetForBounce(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bouncingRadius);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D other)
    {
        if(pierceAmount > 0 && other.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if(isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        
        canRotate = false;

        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if(isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimerDuration);
    }
}
